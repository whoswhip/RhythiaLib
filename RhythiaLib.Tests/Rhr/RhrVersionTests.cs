using RhythiaLib.Rhr;

namespace RhythiaLib.Tests.Rhr
{
    public sealed class RhrVersionTests
    {
        [Fact]
        public void LegacyTimeVersion_RoundTripsFrameTime()
        {
            RhrReplay replay = RhrTestData.CreateCurrentReplay();
            replay.Version = RhrVersions.Int32Time - 1;
            replay.Frames =
            [
                new RhrReplayFrame
                {
                    Time = 1234,
                    PositionX = 0.5f,
                    PositionY = 0.2f,
                    Health = 1f,
                    IsImportantFrame = 0,
                },
            ];

            byte[] bytes = RhrFile.Encode(replay);

            RhrReplay decoded = RhrFile.Decode(bytes);
            Assert.Equal(1234, decoded.Frames[0].Time);
        }

        [Fact]
        public void LegacyYVersion_PreservesY()
        {
            RhrReplay replay = RhrTestData.CreateCurrentReplay();
            replay.Version = RhrVersions.NegateY - 1;
            replay.Frames =
            [
                new RhrReplayFrame
                {
                    Time = 1234,
                    PositionX = 0.5f,
                    PositionY = 0.2f,
                    Health = 1f,
                    IsImportantFrame = 0,
                },
            ];

            byte[] bytes = RhrFile.Encode(replay);

            RhrReplay decoded = RhrFile.Decode(bytes);
            Assert.Equal(0.2f, decoded.Frames[0].PositionY);
        }

        [Fact]
        public void VersionBeforeExtendedFields_UsesDefaults()
        {
            RhrReplay replay = RhrTestData.CreateCurrentReplay();

            replay.Version = RhrVersions.ExtendedFields - 1;

            byte[] bytes = RhrFile.Encode(replay);

            RhrReplay decoded = RhrFile.Decode(bytes);

            Assert.True(decoded.ScoreData.Passed);
            Assert.Equal("[]", decoded.ScoreData.ModsJson);
            Assert.False(decoded.ScoreData.Spin);
            Assert.Equal(0f, decoded.ScoreData.Speed);
            Assert.Equal(0, decoded.ScoreData.TotalScore);
        }
    }
}
