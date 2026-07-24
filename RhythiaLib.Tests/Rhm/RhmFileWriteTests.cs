using System.IO.Compression;
using System.Text.Json;
using RhythiaLib.Maps;
using RhythiaLib.Rhm;

namespace RhythiaLib.Tests.Rhm
{
    public sealed class RhmFileWriteTests
    {
        [Fact]
        public void Encode_WritesExpectedArchiveLayout()
        {
            byte[] data = RhmFile.Encode(RhmFixtureData.CreateMap());

            using var stream = new MemoryStream(data);
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
            ZipArchiveEntry mapEntry = Assert.Single(
                archive.Entries,
                entry => entry.FullName == "map"
            );
            Assert.NotNull(archive.GetEntry("audio.mp3"));
            Assert.NotNull(archive.GetEntry("cover.png"));

            using Stream mapStream = mapEntry.Open();
            using JsonDocument json = JsonDocument.Parse(mapStream);
            Assert.Equal("audio.mp3", json.RootElement.GetProperty("AudioFileName").GetString());
            Assert.Equal("cover.png", json.RootElement.GetProperty("ImagePath").GetString());
            Assert.Equal(
                JsonValueKind.Number,
                json.RootElement.GetProperty("Difficulty").ValueKind
            );
        }

        [Fact]
        public void Encode_ConvertsCoordinatesAndDecodeRestoresThem()
        {
            RhythiaMap map = RhmFixtureData.CreateMap();
            map.Notes =
            [
                new RhythiaNote
                {
                    TimeMilliseconds = 0,
                    X = 0,
                    Y = 0,
                },
                new RhythiaNote
                {
                    TimeMilliseconds = 1,
                    X = -1,
                    Y = 1,
                },
                new RhythiaNote
                {
                    TimeMilliseconds = 2,
                    X = 1,
                    Y = -1,
                },
            ];

            byte[] data = RhmFile.Encode(map);
            using (var stream = new MemoryStream(data))
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
            using (Stream mapStream = archive.GetEntry("map")!.Open())
            using (JsonDocument json = JsonDocument.Parse(mapStream))
            {
                JsonElement notes = json.RootElement.GetProperty("Notes");
                AssertStored(notes[0], 1, 1);
                AssertStored(notes[1], 0, 0);
                AssertStored(notes[2], 2, 2);
            }

            RhythiaMap decoded = RhmFile.Decode(data);
            Assert.Equal((0f, 0f), (decoded.Notes[0].X, decoded.Notes[0].Y));
            Assert.Equal((-1f, 1f), (decoded.Notes[1].X, decoded.Notes[1].Y));
            Assert.Equal((1f, -1f), (decoded.Notes[2].X, decoded.Notes[2].Y));
        }

        [Fact]
        public void Write_LeavesCallerStreamOpen()
        {
            using var stream = new MemoryStream();
            RhmFile.Write(stream, RhmFixtureData.CreateMap());
            Assert.True(stream.CanWrite);
        }

        private static void AssertStored(JsonElement note, float x, float y)
        {
            Assert.Equal(x, note.GetProperty("X").GetSingle());
            Assert.Equal(y, note.GetProperty("Y").GetSingle());
        }
    }
}
