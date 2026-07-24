using RhythiaLib.Maps;
using RhythiaLib.Rhm;

namespace RhythiaLib.Tests.Rhm
{
    public sealed class RhmFileReadTests
    {
        [Theory]
        [MemberData(nameof(RhmFixtureData.FixturePaths), MemberType = typeof(RhmFixtureData))]
        public void Read_GameFixture_ReturnsValidMap(string path)
        {
            RhythiaMap map = RhmFile.Read(path);

            Assert.False(string.IsNullOrWhiteSpace(map.Title));
            Assert.InRange(map.Difficulty, 1, 5);
            Assert.NotEmpty(map.Notes);
            Assert.NotNull(map.Audio);
            Assert.NotEmpty(map.Audio);
            Assert.NotNull(map.Cover);
            Assert.NotEmpty(map.Cover);

            for (int index = 0; index < map.Notes.Count; index++)
            {
                RhythiaNote note = map.Notes[index];
                Assert.Equal(index, note.Index);
                Assert.True(float.IsFinite(note.X));
                Assert.True(float.IsFinite(note.Y));
                if (index > 0)
                    Assert.True(map.Notes[index - 1].TimeMilliseconds <= note.TimeMilliseconds);
            }
        }

        [Fact]
        public void Read_KnownFixture_ReturnsKnownMetadata()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Rhm", "Fixtures", "quantum.rhm");

            RhythiaMap map = RhmFile.Read(path);

            Assert.Equal("5860246058480dfb", map.Id);
            Assert.Equal("nature", map.Artist);
            Assert.Equal("chirp", map.Title);
            Assert.Equal(465, map.LengthMilliseconds);
            Assert.Equal(1, map.Difficulty);
            Assert.Equal(["whip"], map.Mappers);
        }

        [Fact]
        public void Read_LeavesCallerStreamOpen()
        {
            byte[] data = RhmFixtureData.CreateArchive(RhmFixtureData.ValidMapJson);
            using var stream = new MemoryStream(data);

            _ = RhmFile.Read(stream);

            Assert.True(stream.CanRead);
        }
    }
}
