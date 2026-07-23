using System.Text;

namespace RhythiaLib.Rhr.Internal
{
    internal static class RhrWriter
    {
        public static void Write(Stream stream, RhrReplay replay)
        {
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(replay);

            if (!stream.CanWrite)
                throw new ArgumentException("The stream must be writable.", nameof(stream));

            RhrValidation.ValidateReplay(replay);

            using var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);

            writer.Write(replay.Version);

            RhrScoreDataCodec.Write(writer, replay.ScoreData, replay.Version);

            writer.Write(replay.Frames.Count);

            foreach (RhrReplayFrame frame in replay.Frames)
            {
                RhrFrameCodec.Write(writer, frame, replay.Version);
            }
        }
    }
}
