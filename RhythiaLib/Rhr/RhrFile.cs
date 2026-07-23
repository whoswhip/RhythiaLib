using RhythiaLib.Rhr.Internal;

namespace RhythiaLib.Rhr
{
    public static class RhrFile
    {
        public static RhrReplay Read(string path, RhrReadOptions? options = null)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(path);

            using FileStream stream = File.OpenRead(path);
            return Read(stream, options);
        }

        public static RhrReplay Read(Stream stream, RhrReadOptions? options = null)
        {
            return RhrReader.Read(stream, options ?? new RhrReadOptions());
        }

        public static void Write(string path, RhrReplay replay)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
            using FileStream stream = File.OpenWrite(path);
            Write(stream, replay);
        }

        public static void Write(Stream stream, RhrReplay replay)
        {
            RhrWriter.Write(stream, replay);
        }

        public static byte[] Encode(RhrReplay replay)
        {
            using var stream = new MemoryStream();
            Write(stream, replay);
            return stream.ToArray();
        }

        public static RhrReplay Decode(ReadOnlySpan<byte> data, RhrReadOptions? options = null)
        {
            byte[] buffer = data.ToArray();
            using var stream = new MemoryStream(buffer, writable: false);

            return Read(stream, options);
        }
    }
}
