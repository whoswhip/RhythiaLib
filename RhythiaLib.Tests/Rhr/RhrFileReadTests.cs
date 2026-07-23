using RhythiaLib.Rhr;

namespace RhythiaLib.Tests.Rhr
{
    public sealed class RhrFileReadTests
    {
        [Fact]
        public void Decode_ValidReplay_ReturnsExpectedValues()
        {
            RhrReplay original = RhrTestData.CreateCurrentReplay();
            byte[] bytes = RhrFile.Encode(original);
            File.WriteAllBytes("test.rhr", bytes);
            RhrReplay decoded = RhrFile.Decode(bytes);

            Assert.Equal(original.Version, decoded.Version);
            Assert.Equal(original.ScoreData.TimestampTicks, decoded.ScoreData.TimestampTicks);
            Assert.Equal("TestPlayerName", decoded.ScoreData.PlayerName);
            Assert.Equal("test_legacy_map", decoded.ScoreData.LegacyMapId);
            Assert.Equal(1234, decoded.ScoreData.MapId);
            Assert.Equal(1f, decoded.ScoreData.Accuracy);
            Assert.Equal(2, decoded.Frames.Count);
            Assert.Equal(16, decoded.Frames[1].Time);
            Assert.Equal(-0.5f, decoded.Frames[1].PositionY);
        }

        [Fact]
        public void Read_LeavesCallerStreamOpen()
        {
            byte[] bytes = RhrFile.Encode(RhrTestData.CreateCurrentReplay());

            using var stream = new MemoryStream(bytes);
            _ = RhrFile.Read(stream);
            Assert.True(stream.CanRead);
        }
    }
}
