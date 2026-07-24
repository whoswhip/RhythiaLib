using System.Text.Json;
using RhythiaLib.Maps;
using RhythiaLib.Rhm;

namespace RhythiaLib.Tests.Rhm
{
    public sealed class RhmCorruptionTests
    {
        [Fact]
        public void Decode_NonZip_Throws()
        {
            Assert.Throws<InvalidRhmFormatException>(() => RhmFile.Decode("not a zip"u8));
        }

        [Fact]
        public void Decode_MissingMap_Throws()
        {
            Assert.Throws<InvalidRhmFormatException>(() =>
                RhmFile.Decode(RhmFixtureData.CreateArchive("", includeMap: false))
            );
        }

        [Fact]
        public void Decode_InvalidMapJson_Throws()
        {
            Assert.Throws<InvalidRhmFormatException>(() =>
                RhmFile.Decode(RhmFixtureData.CreateArchive("{"))
            );
        }

        [Fact]
        public void Decode_MissingAudio_Throws()
        {
            Assert.Throws<InvalidRhmFormatException>(() =>
                RhmFile.Decode(
                    RhmFixtureData.CreateArchive(RhmFixtureData.ValidMapJson, includeAudio: false)
                )
            );
        }

        [Fact]
        public void Decode_MissingCover_Throws()
        {
            Assert.Throws<InvalidRhmFormatException>(() =>
                RhmFile.Decode(
                    RhmFixtureData.CreateArchive(RhmFixtureData.ValidMapJson, includeCover: false)
                )
            );
        }

        [Fact]
        public void Decode_MapOverLimit_Throws()
        {
            Assert.Throws<InvalidRhmFormatException>(() =>
                RhmFile.Decode(
                    RhmFixtureData.CreateArchive(RhmFixtureData.ValidMapJson),
                    new RhmReadOptions { MaximumMapSize = 1 }
                )
            );
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        public void Decode_InvalidDifficulty_Throws(int difficulty)
        {
            string json = Replace("Difficulty", difficulty);
            Assert.Throws<InvalidRhmFormatException>(() =>
                RhmFile.Decode(RhmFixtureData.CreateArchive(json))
            );
        }

        [Fact]
        public void Decode_NegativeDuration_Throws()
        {
            string json = Replace("Duration", -1);
            Assert.Throws<InvalidRhmFormatException>(() =>
                RhmFile.Decode(RhmFixtureData.CreateArchive(json))
            );
        }

        [Fact]
        public void Decode_ExcessiveNoteCount_Throws()
        {
            Assert.Throws<InvalidRhmFormatException>(() =>
                RhmFile.Decode(
                    RhmFixtureData.CreateArchive(RhmFixtureData.ValidMapJson),
                    new RhmReadOptions { MaximumNoteCount = 0 }
                )
            );
        }

        [Fact]
        public void Decode_NegativeNoteTime_Throws()
        {
            string json = RhmFixtureData.ValidMapJson.Replace(
                "\"Time\":10",
                "\"Time\":-1",
                StringComparison.Ordinal
            );
            Assert.Throws<InvalidRhmFormatException>(() =>
                RhmFile.Decode(RhmFixtureData.CreateArchive(json))
            );
        }

        [Fact]
        public void Decode_NonFiniteNoteCoordinate_Throws()
        {
            string json = RhmFixtureData.ValidMapJson.Replace(
                "\"X\":1",
                "\"X\":1e100",
                StringComparison.Ordinal
            );
            Assert.Throws<InvalidRhmFormatException>(() =>
                RhmFile.Decode(RhmFixtureData.CreateArchive(json))
            );
        }

        [Fact]
        public void Read_UnreadableStream_Throws()
        {
            using var stream = new UnreadableStream();
            Assert.Throws<ArgumentException>(() => RhmFile.Read(stream));
        }

        [Fact]
        public void Write_UnwritableStream_Throws()
        {
            using var stream = new MemoryStream([], writable: false);
            Assert.Throws<ArgumentException>(() =>
                RhmFile.Write(stream, RhmFixtureData.CreateMap())
            );
        }

        [Fact]
        public void Encode_MissingRequiredMedia_Throws()
        {
            RhythiaMap map = RhmFixtureData.CreateMap();
            map.Audio = null;
            Assert.Throws<ArgumentException>(() => RhmFile.Encode(map));

            map.Audio = [];
            map.Cover = null;
            Assert.Throws<ArgumentException>(() => RhmFile.Encode(map));
        }

        private static string Replace(string propertyName, int value)
        {
            using JsonDocument document = JsonDocument.Parse(RhmFixtureData.ValidMapJson);
            var values = new Dictionary<string, object?>();
            foreach (JsonProperty property in document.RootElement.EnumerateObject())
            {
                values[property.Name] =
                    property.Name == propertyName ? value : property.Value.Clone();
            }

            return JsonSerializer.Serialize(values);
        }

        private sealed class UnreadableStream : MemoryStream
        {
            public override bool CanRead => false;
        }
    }
}
