using RhythiaLib.Rhr;

namespace RhythiaLib.Tests.Rhr
{
    public sealed class RhrRoundTripTests
    {
        [Fact]
        public void SyntheticReplay_RoundTripsExactly()
        {
            RhrReplay original = RhrTestData.CreateCurrentReplay();

            byte[] firstEncoding = RhrFile.Encode(original);

            RhrReplay decoded = RhrFile.Decode(firstEncoding);

            byte[] secondEncoding = RhrFile.Encode(original);

            Assert.Equal(firstEncoding, secondEncoding);
        }

        [Theory]
        [MemberData(nameof(RhrTestData.FixturePaths), MemberType = typeof(RhrTestData))]
        public void RealFixture_RoundTripsExactly(string path)
        {
            byte[] originalBytes = File.ReadAllBytes(path);

            RhrReplay replay = RhrFile.Decode(originalBytes);
            byte[] encodedBytes = RhrFile.Encode(replay);

            Assert.Equal(originalBytes, encodedBytes);
        }
    }
}
