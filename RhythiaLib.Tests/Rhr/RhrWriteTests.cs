using RhythiaLib.Rhr;

namespace RhythiaLib.Tests.Rhr
{
    public sealed class RhrWriteTests
    {
        [Fact]
        public void Write_LeavesCallerStreamOpen()
        {
            RhrReplay replay = RhrTestData.CreateCurrentReplay();

            using var stream = new MemoryStream();
            RhrFile.Write(stream, replay);

            Assert.True(stream.CanWrite);
            Assert.NotEqual(0, stream.Length);
        }

        [Fact]
        public void Encode_WritesFrameCount()
        {
            RhrReplay replay = RhrTestData.CreateCurrentReplay();

            byte[] data = RhrFile.Encode(replay);
            RhrReplay decoded = RhrFile.Decode(data);

            Assert.Equal(replay.Frames.Count, decoded.Frames.Count);
        }

        [Fact]
        public void Write_NonFiniteCoordinate_Throws()
        {
            RhrReplay replay = RhrTestData.CreateCurrentReplay();
            replay.Frames[0].PositionX = float.NaN;

            using var stream = new MemoryStream();

            Assert.Throws<ArgumentException>(() => RhrFile.Write(stream, replay));
        }
    }
}
