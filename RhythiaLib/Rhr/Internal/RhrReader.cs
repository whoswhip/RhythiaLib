using System.Text;

namespace RhythiaLib.Rhr.Internal
{
    internal static class RhrReader
    {
        public static RhrReplay Read(Stream stream, RhrReadOptions options)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(options);

            if (!stream.CanRead)
                throw new ArgumentException("The stream must be readable.", nameof(stream));

            using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

            try
            {
                int version = reader.ReadInt32();
                RhrVersions.Validate(version);

                RhrScoreData scoreData = RhrScoreDataCodec.Read(reader, version);

                int frameCount = reader.ReadInt32();

                RhrValidation.ValidateFrameCount(stream, frameCount, options);

                var frames = new List<RhrReplayFrame>(frameCount);

                for (int index = 0; index < frameCount; index++)
                {
                    RhrReplayFrame frame = RhrFrameCodec.Read(reader, version);
                    frames.Add(frame);
                }

                RhrValidation.ValidateEndOfStream(stream);

                return new RhrReplay
                {
                    Version = version,
                    ScoreData = scoreData,
                    Frames = frames,
                };
            }
            catch (EndOfStreamException exception)
            {
                throw new InvalidRhrFormatException("The replay is truncated.", exception);
            }
            catch (IOException exception)
            {
                throw new InvalidRhrFormatException("The replay could not be read.", exception);
            }
        }
    }
}
