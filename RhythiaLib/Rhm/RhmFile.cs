using RhythiaLib.Maps;
using RhythiaLib.Rhm.Internal;

namespace RhythiaLib.Rhm
{
    public static class RhmFile
    {
        public static RhythiaMap Read(string path, RhmReadOptions? options = null)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(path);

            using FileStream stream = File.OpenRead(path);
            return Read(stream, options);
        }

        public static RhythiaMap Read(Stream stream, RhmReadOptions? options = null)
        {
            return RhmArchiveCodec.Read(stream, options ?? new RhmReadOptions());
        }

        public static void Write(string path, RhythiaMap map)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
            using FileStream stream = File.OpenWrite(path);
            Write(stream, map);
        }

        public static void Write(Stream stream, RhythiaMap map)
        {
            RhmArchiveCodec.Write(stream, map);
        }

        public static byte[] Encode(RhythiaMap map)
        {
            using var stream = new MemoryStream();
            Write(stream, map);
            return stream.ToArray();
        }

        public static RhythiaMap Decode(ReadOnlySpan<byte> data, RhmReadOptions? options = null)
        {
            byte[] buffer = data.ToArray();
            using var stream = new MemoryStream(buffer, writable: false);

            return Read(stream, options);
        }
    }
}
