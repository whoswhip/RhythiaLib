namespace RhythiaLib.Rhr
{
    public sealed class RhrReplay
    {
        public int Version { get; set; }
        public RhrScoreData ScoreData { get; set; } = new();
        public List<RhrReplayFrame> Frames { get; set; } = [];
    }
}
