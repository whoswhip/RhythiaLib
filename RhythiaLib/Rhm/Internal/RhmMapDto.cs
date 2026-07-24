namespace RhythiaLib.Rhm.Internal
{
    internal sealed class RhmMapDto
    {
        public string? OnlineId { get; set; }
        public object? OnlineStatus { get; set; }
        public string? LegacyId { get; set; }
        public string SongName { get; set; } = string.Empty;
        public List<string> Mappers { get; set; } = [];
        public string Title { get; set; } = string.Empty; // no clue what the difference between this and SongName is
        public int Duration { get; set; }
        public int Difficulty { get; set; }
        public string CustomDifficultyName { get; set; } = string.Empty;
        public double StarRating { get; set; }
        public List<RhmNoteDto> Notes { get; set; } = [];
        public string ImagePath { get; set; } = string.Empty;
        public string AudioFileName { get; set; } = string.Empty;
    }
}
