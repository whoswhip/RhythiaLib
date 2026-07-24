using System.Text.Json;

namespace RhythiaLib.Rhm.Internal
{
    internal static class RhmMapCodec
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = false,
            WriteIndented = false,
        };

        public static RhmMapDto Read(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream);

            try
            {
                RhmMapDto? map = JsonSerializer.Deserialize<RhmMapDto>(stream, JsonOptions);
                return map
                    ?? throw new InvalidRhmFormatException(
                        "The RHM map JSON has a null root object."
                    );
            }
            catch (JsonException exception)
            {
                throw new InvalidRhmFormatException(
                    "The RHM map entry contains malformed JSON.",
                    exception
                );
            }
        }

        public static void Write(Stream stream, RhmMapDto map)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(map);
            JsonSerializer.Serialize(stream, map, JsonOptions);
        }
    }
}
