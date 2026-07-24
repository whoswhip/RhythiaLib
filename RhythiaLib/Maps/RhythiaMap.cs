namespace RhythiaLib.Maps
{
    public sealed class RhythiaMap
    {
        public string Id { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string ArtistLink { get; set; } = string.Empty;
        public string ArtistPlatform { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public float Rating { get; set; }
        public List<string> Mappers { get; set; } = [];
        public int Difficulty { get; set; }
        public string DifficultyName { get; set; } = string.Empty;
        public int LengthMilliseconds { get; set; }
        public List<RhythiaNote> Notes { get; set; } = [];
        public byte[]? Audio { get; set; }
        public string? AudioExtension { get; set; }
        public byte[]? Cover { get; set; }
        public byte[]? Video { get; set; }
    }
}
