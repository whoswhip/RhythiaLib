using System.IO.Compression;
using RhythiaLib.Maps;

namespace RhythiaLib.Rhm.Internal
{
    internal static class RhmValidation
    {
        public static void ValidateReadOptions(RhmReadOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            if (options.MaximumMapSize < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(options),
                    "Maximum map size must be nonnegative."
                );
            if (options.MaximumAudioSize < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(options),
                    "Maximum audio size must be nonnegative."
                );
            if (options.MaximumCoverSize < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(options),
                    "Maximum cover size must be nonnegative."
                );
            if (options.MaximumNoteCount < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(options),
                    "Maximum note count must be nonnegative."
                );
        }

        public static void ValidateEntrySize(ZipArchiveEntry entry, long maximumSize, string kind)
        {
            ArgumentNullException.ThrowIfNull(entry);

            if (entry.Length > maximumSize)
            {
                throw new InvalidRhmFormatException(
                    $"The RHM {kind} entry size of {entry.Length} bytes exceeds "
                        + $"the configured maximum of {maximumSize} bytes."
                );
            }

            if (entry.Length > int.MaxValue)
            {
                throw new InvalidRhmFormatException(
                    $"The RHM {kind} entry is too large to load into memory."
                );
            }
        }

        public static void ValidateDto(RhmMapDto map, RhmReadOptions options)
        {
            if (map.Difficulty is < 1 or > 5)
                throw new InvalidRhmFormatException(
                    $"RHM difficulty {map.Difficulty} is outside the supported range 1 through 5."
                );

            if (map.Duration < 0)
                throw new InvalidRhmFormatException("The RHM map duration is negative.");

            if (map.Mappers is null)
                throw new InvalidRhmFormatException("The RHM mappers collection is null.");

            if (map.Notes is null)
                throw new InvalidRhmFormatException("The RHM notes collection is null.");

            if (map.Notes.Count > options.MaximumNoteCount)
            {
                throw new InvalidRhmFormatException(
                    $"The RHM note count of {map.Notes.Count} exceeds "
                        + $"the configured maximum of {options.MaximumNoteCount}."
                );
            }

            if (!double.IsFinite(map.StarRating))
                throw new InvalidRhmFormatException("The RHM star rating is not finite.");

            for (int index = 0; index < map.Notes.Count; index++)
            {
                RhmNoteDto note =
                    map.Notes[index]
                    ?? throw new InvalidRhmFormatException($"RHM note {index} is null.");

                if (note.Time < 0)
                    throw new InvalidRhmFormatException($"RHM note {index} has a negative time.");

                if (!float.IsFinite(note.X) || !float.IsFinite(note.Y))
                {
                    throw new InvalidRhmFormatException(
                        $"RHM note {index} contains a non-finite coordinate."
                    );
                }
            }
        }

        public static void ValidateMapForWriting(RhythiaMap map)
        {
            ArgumentNullException.ThrowIfNull(map);

            if (map.Audio is null)
                throw new ArgumentException("RHM maps require audio data.", nameof(map));

            if (map.Cover is null)
                throw new ArgumentException("RHM maps require cover-image data.", nameof(map));

            if (map.Difficulty is < 1 or > 5)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(map),
                    map.Difficulty,
                    "RHM difficulty must be between 1 and 5."
                );
            }

            if (map.Title is null)
                throw new ArgumentException("The map title cannot be null.", nameof(map));

            if (map.Mappers is null)
                throw new ArgumentException(
                    "The map mappers collection cannot be null.",
                    nameof(map)
                );

            if (map.Notes is null)
                throw new ArgumentException(
                    "The map notes collection cannot be null.",
                    nameof(map)
                );

            if (!float.IsFinite(map.Rating))
                throw new ArgumentException("The map rating must be finite.", nameof(map));

            if (map.LengthMilliseconds < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(map),
                    map.LengthMilliseconds,
                    "The map length cannot be negative."
                );

            for (int index = 0; index < map.Notes.Count; index++)
            {
                RhythiaNote note =
                    map.Notes[index]
                    ?? throw new ArgumentException($"Map note {index} is null.", nameof(map));

                if (note.TimeMilliseconds < 0)
                {
                    throw new ArgumentException(
                        $"Map note {index} has a negative time.",
                        nameof(map)
                    );
                }

                if (!float.IsFinite(note.X) || !float.IsFinite(note.Y))
                {
                    throw new ArgumentException(
                        $"Map note {index} contains a non-finite coordinate.",
                        nameof(map)
                    );
                }
            }
        }
    }
}
