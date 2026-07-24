using System.IO.Compression;
using System.Text;
using RhythiaLib.Maps;

namespace RhythiaLib.Tests.Rhm
{
    internal static class RhmFixtureData
    {
        public const string ValidMapJson = """
            {"OnlineId":null,"OnlineStatus":null,"LegacyId":"test-id","SongName":"Artist - Song","Mappers":["Mapper"],"Title":"Artist - Song","Duration":1000,"Difficulty":3,"CustomDifficultyName":"Test","StarRating":4.5,"Notes":[{"Time":10,"X":1,"Y":1}],"ImagePath":"cover","AudioFileName":"song.mp3"}
            """;

        public static TheoryData<string> FixturePaths()
        {
            var data = new TheoryData<string>();

            string directory = Path.Combine(AppContext.BaseDirectory, "Rhm", "Fixtures");

            if (!Directory.Exists(directory))
                return data;

            foreach (
                string path in Directory.EnumerateFiles(
                    directory,
                    "*.rhm",
                    SearchOption.AllDirectories
                )
            )
            {
                data.Add(path);
            }

            return data;
        }

        public static RhythiaMap CreateMap()
        {
            return new RhythiaMap
            {
                Id = "test-id",
                Artist = "Artist",
                Title = "Artist - Song",
                Rating = 4.5f,
                Mappers = ["Mapper"],
                Difficulty = 3,
                DifficultyName = "Test",
                LengthMilliseconds = 1000,
                Notes =
                [
                    new RhythiaNote
                    {
                        Index = 0,
                        TimeMilliseconds = 10,
                        X = 0,
                        Y = 0,
                    },
                ],
                Audio = [1, 2, 3],
                AudioExtension = ".mp3",
                Cover = [4, 5, 6],
            };
        }

        public static byte[] CreateArchive(
            string mapJson,
            bool includeMap = true,
            bool includeAudio = true,
            bool includeCover = true
        )
        {
            using var stream = new MemoryStream();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
            {
                if (includeMap)
                    WriteEntry(archive, "map", Encoding.UTF8.GetBytes(mapJson));
                if (includeAudio)
                    WriteEntry(archive, "audio", [1, 2, 3]);
                if (includeCover)
                    WriteEntry(archive, "cover", [4, 5, 6]);
            }

            return stream.ToArray();
        }

        private static void WriteEntry(ZipArchive archive, string name, byte[] data)
        {
            ZipArchiveEntry entry = archive.CreateEntry(name);
            using Stream stream = entry.Open();
            stream.Write(data);
        }
    }
}
