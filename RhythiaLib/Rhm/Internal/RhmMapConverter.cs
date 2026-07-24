using RhythiaLib.Maps;

namespace RhythiaLib.Rhm.Internal
{
    internal static class RhmMapConverter
    {
        public static RhythiaMap ToModel(RhmMapDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            string displayTitle = dto.Title ?? string.Empty;
            string[] titleParts = displayTitle.Split(" - ", 2, StringSplitOptions.None);

            var notes = new List<RhythiaNote>(dto.Notes.Count);
            foreach (RhmNoteDto note in dto.Notes)
            {
                notes.Add(
                    new RhythiaNote
                    {
                        Index = notes.Count,
                        TimeMilliseconds = note.Time,
                        X = note.X - 1f,
                        Y = 1f - note.Y,
                    }
                );
            }

            notes.Sort(
                static (left, right) =>
                {
                    int timeComparison = left.TimeMilliseconds.CompareTo(right.TimeMilliseconds);
                    return timeComparison != 0 ? timeComparison : left.Index.CompareTo(right.Index);
                }
            );
            for (int index = 0; index < notes.Count; index++)
                notes[index].Index = index;

            return new RhythiaMap
            {
                Id = !string.IsNullOrEmpty(dto.LegacyId)
                    ? dto.LegacyId
                    : dto.OnlineId ?? string.Empty,
                Artist = titleParts.Length == 2 ? titleParts[0] : string.Empty,
                Title = titleParts.Length == 2 ? titleParts[1] : displayTitle,
                Rating = (float)dto.StarRating,
                Mappers = [.. dto.Mappers],
                Difficulty = dto.Difficulty,
                DifficultyName = dto.CustomDifficultyName ?? string.Empty,
                LengthMilliseconds = dto.Duration,
                Notes = notes,
            };
        }

        public static RhmMapDto ToDto(RhythiaMap map, string audioEntryName, string coverEntryName)
        {
            ArgumentNullException.ThrowIfNull(map);

            string displayTitle = string.IsNullOrWhiteSpace(map.Artist)
                ? map.Title
                : $"{map.Artist} - {map.Title}";

            var notes = new List<RhmNoteDto>(map.Notes.Count);
            foreach (RhythiaNote note in map.Notes)
            {
                notes.Add(
                    new RhmNoteDto
                    {
                        Time = note.TimeMilliseconds,
                        X = note.X + 1f,
                        Y = 1f - note.Y,
                    }
                );
            }

            return new RhmMapDto
            {
                LegacyId = map.Id,
                SongName = displayTitle,
                Mappers = [.. map.Mappers],
                Title = displayTitle,
                Duration = map.LengthMilliseconds,
                Difficulty = map.Difficulty,
                CustomDifficultyName = map.DifficultyName,
                StarRating = map.Rating,
                Notes = notes,
                AudioFileName = audioEntryName,
                ImagePath = coverEntryName,
            };
        }
    }
}
