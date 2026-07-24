namespace RhythiaLib.Rhm
{
    public sealed class RhmReadOptions
    {
        public long MaximumMapSize { get; init; } = 16 * 1024 * 1024; // 16 MB
        public long MaximumAudioSize { get; init; } = 128 * 1024 * 1024; // 128 MB
        public long MaximumCoverSize { get; init; } = 32 * 1024 * 1024; // 32 MB
        public int MaximumNoteCount { get; init; } = 5_000_000;
    }
}
