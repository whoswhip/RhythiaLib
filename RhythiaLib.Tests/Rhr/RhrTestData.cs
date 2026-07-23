using RhythiaLib.Rhr;

namespace RhythiaLib.Tests.Rhr
{
    internal static class RhrTestData
    {
        public static RhrReplay CreateCurrentReplay()
        {
            return new RhrReplay
            {
                Version = RhrVersions.BeatmapHash,
                ScoreData = new RhrScoreData
                {
                    TimestampTicks = new DateTime(
                        2026,
                        7,
                        23,
                        12,
                        0,
                        0,
                        DateTimeKind.Unspecified
                    ).Ticks,
                    PlayerName = "TestPlayerName",
                    LegacyMapId = "test_legacy_map",
                    MapId = 1234,
                    StartFrom = 0,
                    Mode = "online_profile",
                    Passed = true,
                    ModsJson = "[]",
                    Spin = false,
                    Speed = 1f,
                    TotalScore = 1_000_000,
                    Accuracy = 1f,
                    Hits = 100,
                    Misses = 0,
                    Points = 100f,
                    Failed = false,
                    FailTime = -1,
                    BeatmapHash =
                        "0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcdef",
                },
                Frames =
                [
                    new RhrReplayFrame
                    {
                        Time = 0,
                        PositionX = 0f,
                        PositionY = 0f,
                        Health = 1f,
                        IsImportantFrame = 0,
                    },
                    new RhrReplayFrame
                    {
                        Time = 16,
                        PositionX = 0.7f,
                        PositionY = -0.5f,
                        Health = 0.85f,
                        IsImportantFrame = 1,
                    },
                ],
            };
        }

        public static TheoryData<string> FixturePaths()
        {
            var data = new TheoryData<string>();
            string directory = Path.Combine(AppContext.BaseDirectory, "Rhr", "Fixtures");

            if (!Directory.Exists(directory))
                return data;

            foreach (string path in Directory.EnumerateFiles(directory, "*.rhr"))
            {
                data.Add(path);
            }

            return data;
        }
    }
}
