using RhythiaLib.Rhr.Internal;

namespace RhythiaLib.Rhr
{
    public static class RhrFile
    {

        /// <summary>
        /// Reads a replay from the specified file.
        /// </summary>
        /// <param name="path">The path of the RHR file to read.</param>
        /// <param name="options">Optional settings that control resource limits.</param>
        /// <returns>The replay decoded from the file.</returns>
        public static RhrReplay Read(string path, RhrReadOptions? options = null)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(path);

            using FileStream stream = File.OpenRead(path);
            return Read(stream, options);
        }

        /// <summary>
        /// Read a replay from the specified stream.
        /// </summary>
        /// <param name="stream">A readable stream at the beginning of a replay.</param>
        /// <param name="options">Optional settings that control resource limits.</param>
        /// <returns>The replay decoded from the stream.</returns>
        /// <remarks>This method will not close or dispose <paramref name="stream"/></remarks>
        public static RhrReplay Read(Stream stream, RhrReadOptions? options = null)
        {
            return RhrReader.Read(stream, options ?? new RhrReadOptions());
        }

        /// <summary>
        /// Writes a replay to the specified file.
        /// </summary>
        /// <param name="path">The destination file path. Will overwrite any existing file.</param>
        /// <param name="replay">The replay to encode and write.</param>
        public static void Write(string path, RhrReplay replay)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(path);
            using FileStream stream = File.OpenWrite(path);
            Write(stream, replay);
        }

        /// <summary>
        /// Writes a replay to the specified stream.
        /// </summary>
        /// <param name="stream">A writable stream that will recieve the encoded replay.</param>
        /// <param name="replay">The replay to encode and write.</param>
        /// <remarks>This method writes at the stream's current position and does not close or dispose <paramref name="stream"/></remarks>
        public static void Write(Stream stream, RhrReplay replay)
        {
            RhrWriter.Write(stream, replay);
        }

        /// <summary>
        /// Encodes and RHR replay into a new byte array.
        /// </summary>
        /// <param name="replay">The replay to encode.</param>
        /// <returns>A byte array containing the encoded replay.</returns>
        public static byte[] Encode(RhrReplay replay)
        {
            using var stream = new MemoryStream();
            Write(stream, replay);
            return stream.ToArray();
        }

        /// <summary>
        /// Decodes a replay from the specified bytes.
        /// </summary>
        /// <param name="data">The encoded replay data.</param>
        /// <param name="options">Optional settings that control resource limits.</param>
        /// <returns>The replay decoded from <paramref name="data"/>ww</returns>
        public static RhrReplay Decode(ReadOnlySpan<byte> data, RhrReadOptions? options = null)
        {
            byte[] buffer = data.ToArray();
            using var stream = new MemoryStream(buffer, writable: false);

            return Read(stream, options);
        }
    }
}
