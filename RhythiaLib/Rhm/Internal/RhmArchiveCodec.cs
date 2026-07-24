using System.IO.Compression;
using RhythiaLib.Maps;

namespace RhythiaLib.Rhm.Internal
{
    internal static class RhmArchiveCodec
    {
        private const string MapEntryName = "map";
        private const string DefaultCoverEntryName = "cover.png";

        public static RhythiaMap Read(Stream stream, RhmReadOptions options)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(options);

            if (!stream.CanRead)
                throw new ArgumentException("The stream must be readable.", nameof(stream));

            RhmValidation.ValidateReadOptions(options);

            try
            {
                using var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true);

                ZipArchiveEntry mapEntry =
                    archive.GetEntry(MapEntryName)
                    ?? throw new InvalidRhmFormatException(
                        "The RHM archive is missing its map entry."
                    );
                RhmValidation.ValidateEntrySize(mapEntry, options.MaximumMapSize, "map");

                RhmMapDto dto;
                using (Stream mapStream = mapEntry.Open())
                    dto = RhmMapCodec.Read(mapStream);

                RhmValidation.ValidateDto(dto, options);

                ZipArchiveEntry audioEntry =
                    archive.GetEntry("audio")
                    ?? (
                        !string.IsNullOrEmpty(dto.AudioFileName)
                            ? archive.GetEntry(dto.AudioFileName)
                            : null
                    )
                    ?? throw new InvalidRhmFormatException(
                        "The RHM archive is missing its required audio entry."
                    );

                if (string.IsNullOrEmpty(dto.ImagePath))
                {
                    throw new InvalidRhmFormatException(
                        "The RHM map does not specify its required cover entry."
                    );
                }

                ZipArchiveEntry coverEntry =
                    archive.GetEntry(dto.ImagePath)
                    ?? throw new InvalidRhmFormatException(
                        $"The RHM archive is missing its required cover entry '{dto.ImagePath}'."
                    );

                byte[] audio = ReadEntry(audioEntry, options.MaximumAudioSize, "audio");
                byte[] cover = ReadEntry(coverEntry, options.MaximumCoverSize, "cover");

                RhythiaMap map = RhmMapConverter.ToModel(dto);
                map.Audio = audio;
                map.Cover = cover;

                string extension = Path.GetExtension(audioEntry.FullName);
                map.AudioExtension = string.IsNullOrEmpty(extension) ? null : extension;
                return map;
            }
            catch (InvalidRhmFormatException)
            {
                throw;
            }
            catch (InvalidDataException exception)
            {
                throw new InvalidRhmFormatException(
                    "The input is not a valid RHM ZIP archive.",
                    exception
                );
            }
        }

        public static void Write(Stream stream, RhythiaMap map)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(map);

            if (!stream.CanWrite)
                throw new ArgumentException("The stream must be writable.", nameof(stream));

            RhmValidation.ValidateMapForWriting(map);

            string audioEntryName = GetAudioEntryName(map.AudioExtension);
            const string coverEntryName = DefaultCoverEntryName;
            RhmMapDto dto = RhmMapConverter.ToDto(map, audioEntryName, coverEntryName);

            using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);

            ZipArchiveEntry mapEntry = archive.CreateEntry(MapEntryName, CompressionLevel.Optimal);
            using (Stream mapStream = mapEntry.Open())
                RhmMapCodec.Write(mapStream, dto);

            WriteEntry(archive, audioEntryName, map.Audio!);
            WriteEntry(archive, coverEntryName, map.Cover!);
        }

        private static byte[] ReadEntry(ZipArchiveEntry entry, long maximumSize, string kind)
        {
            RhmValidation.ValidateEntrySize(entry, maximumSize, kind);

            using Stream input = entry.Open();
            using var output = new MemoryStream((int)entry.Length);
            input.CopyTo(output);
            return output.ToArray();
        }

        private static void WriteEntry(ZipArchive archive, string name, byte[] data)
        {
            ZipArchiveEntry entry = archive.CreateEntry(name, CompressionLevel.Optimal);
            using Stream output = entry.Open();
            output.Write(data);
        }

        private static string GetAudioEntryName(string? extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                return "audio";

            string trimmed = extension.Trim();
            if (trimmed.IndexOfAny(['/', '\\']) >= 0)
            {
                throw new ArgumentException(
                    "The audio extension cannot contain path separators.",
                    nameof(extension)
                );
            }

            if (trimmed == ".")
                return "audio";

            return trimmed[0] == '.' ? $"audio{trimmed}" : $"audio.{trimmed}";
        }
    }
}
