namespace RhythiaLib.Rhr
{
    public sealed class RhrScoreData
    {
        public long TimestampTicks { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public string LegacyMapId { get; set; } = string.Empty;
        public int MapId { get; set; }
        public int StartFrom { get; set; }
        public string Mode { get; set; } = string.Empty;
        public bool Passed { get; set; } = true;
        public string ModsJson { get; set; } = "[]";
        public bool Spin { get; set; }
        public float Speed { get; set; }
        public long TotalScore { get; set; }
        public float Accuracy { get; set; }
        public int Hits { get; set; }
        public int Misses { get; set; }
        public float Points { get; set; }
        public bool Failed { get; set; }
        public int FailTime { get; set; } = -1;
        public string BeatmapHash { get; set; } = string.Empty;

        public DateTime DatePlayed
        {
            get
            {
                try
                {
                    return new DateTime(TimestampTicks, DateTimeKind.Unspecified);
                }
                catch (ArgumentOutOfRangeException exception)
                {
                    throw new InvalidRhrFormatException(
                        $"Invalid timestamp value: {TimestampTicks}.",
                        exception
                    );
                }
            }
        }
    }
}
