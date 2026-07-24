// version dates & info from:
// https://github.com/yo-ru/rhrParse/blob/69642c3347724c73fc95df18474fe0cf1c1ddb76/include/rhrParse/rhrParse.h#L79-L100

namespace RhythiaLib.Rhr
{
    public static class RhrVersions
    {
        public const int NegateY = 2026_01_18;
        public const int ExtendedFields = 2026_01_25;
        public const int FailTime = 2026_02_22;
        public const int Int32Time = 2026_05_10;
        public const int BeatmapHash = 2026_05_17;

        public const int LatestSupported = BeatmapHash;

        public static void Validate(int version)
        {
            // oldest version isnt validated since idk when the format was made and if it breaks before 2026/01/18
            if (version <= 0)
                throw new InvalidRhrFormatException($"Invalid RHR version: {version}.");
            else if (version > LatestSupported)
                throw new InvalidRhrFormatException($"RHR version {version} is newer than the latest supported version, {version}");
        }
    }
}
