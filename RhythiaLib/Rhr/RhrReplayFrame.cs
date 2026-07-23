namespace RhythiaLib.Rhr
{
    public sealed class RhrReplayFrame
    {
        public int Time { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float Health { get; set; }
        public byte IsImportantFrame { get; set; } // im ngl, i have NO clue what this is/does

        internal float? LegacyWireTime { get; set; }
        internal int? DecodedLegacyTime { get; set; }
    }
}
