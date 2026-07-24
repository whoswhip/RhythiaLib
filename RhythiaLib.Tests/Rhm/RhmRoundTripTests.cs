using RhythiaLib.Maps;
using RhythiaLib.Rhm;

namespace RhythiaLib.Tests.Rhm
{
    public sealed class RhmRoundTripTests
    {
        [Theory]
        [MemberData(nameof(RhmFixtureData.FixturePaths), MemberType = typeof(RhmFixtureData))]
        public void Fixture_LogicalRoundTripPreservesData(string path)
        {
            RhythiaMap original = RhmFile.Read(path);
            RhythiaMap decoded = RhmFile.Decode(RhmFile.Encode(original));

            Assert.Equal(original.Id, decoded.Id);
            Assert.Equal(original.Artist, decoded.Artist);
            Assert.Equal(original.Title, decoded.Title);
            Assert.Equal(original.Rating, decoded.Rating);
            Assert.Equal(original.Mappers, decoded.Mappers);
            Assert.Equal(original.Difficulty, decoded.Difficulty);
            Assert.Equal(original.DifficultyName, decoded.DifficultyName);
            Assert.Equal(original.LengthMilliseconds, decoded.LengthMilliseconds);
            Assert.Equal(original.Audio, decoded.Audio);
            Assert.Equal(original.Cover, decoded.Cover);
            Assert.Equal(original.Notes.Count, decoded.Notes.Count);

            for (int index = 0; index < original.Notes.Count; index++)
            {
                RhythiaNote expected = original.Notes[index];
                RhythiaNote actual = decoded.Notes[index];
                Assert.Equal(expected.Index, actual.Index);
                Assert.Equal(expected.TimeMilliseconds, actual.TimeMilliseconds);
                Assert.Equal(expected.X, actual.X);
                Assert.Equal(expected.Y, actual.Y);
            }
        }
    }
}
