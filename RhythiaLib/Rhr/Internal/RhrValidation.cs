namespace RhythiaLib.Rhr.Internal
{
    internal static class RhrValidation
    {
        public static void ValidateFrameCount(
            Stream streamm,
            int frameCount,
            RhrReadOptions options
        )
        {
            ArgumentNullException.ThrowIfNull(streamm);
            ArgumentNullException.ThrowIfNull(options);

            if (frameCount < 0)
                throw new InvalidRhrFormatException($"Negative frame count: {frameCount}.");

            if (frameCount > options.MaximumFrameCount)
                throw new InvalidRhrFormatException(
                    $"Frame count {frameCount} exceeded the configured maximum of {options.MaximumFrameCount}"
                );

            if (!streamm.CanSeek)
                return;

            long requiredBytes = checked((long)frameCount * RhrFrameCodec.WireSize);
            long remainingBytes = streamm.Length - streamm.Position;

            if (remainingBytes < requiredBytes)
            {
                throw new InvalidRhrFormatException(
                    $"The replay declares {frameCount} frames, "
                        + $"requiring {requiredBytes} bytes, "
                        + $"but only {remainingBytes} bytes remain"
                );
            }
        }

        public static void ValidateEndOfStream(Stream stream)
        {
            if (!stream.CanSeek)
                return;

            if (stream.Position != stream.Length)
            {
                long trailingByteCount = stream.Length - stream.Position;

                throw new InvalidRhrFormatException(
                    $"The replay contains {trailingByteCount} unexpected trailing bytes."
                );
            }
        }

        public static void ValidateReplay(RhrReplay replay)
        {
            ArgumentNullException.ThrowIfNull(replay);
            ArgumentNullException.ThrowIfNull(replay.ScoreData);
            ArgumentNullException.ThrowIfNull(replay.Frames);

            ValidateScoreData(replay.ScoreData);

            for (int index = 0; index < replay.Frames.Count; index++)
            {
                RhrReplayFrame frame =
                    replay.Frames[index]
                    ?? throw new ArgumentException($"Frame {index} is null", nameof(replay));

                ValidateFrame(frame, index);
            }
        }

        private static void ValidateScoreData(RhrScoreData scoreData)
        {
            ArgumentNullException.ThrowIfNull(scoreData.PlayerName);
            ArgumentNullException.ThrowIfNull(scoreData.LegacyMapId);
            ArgumentNullException.ThrowIfNull(scoreData.Mode);
            ArgumentNullException.ThrowIfNull(scoreData.ModsJson);
            ArgumentNullException.ThrowIfNull(scoreData.BeatmapHash);

            if (!float.IsFinite(scoreData.Speed))
                throw new ArgumentException("Replay speed must be finite.");

            if (!float.IsFinite(scoreData.Accuracy))
                throw new ArgumentException("Replay accuracy must be finite.");

            if (!float.IsFinite(scoreData.Points))
                throw new ArgumentException("Replay points must be finite.");

            if (scoreData.Failed && scoreData.FailTime < 0)
                throw new ArgumentException("A failed replay must have a non-negative fail time.");
        }

        private static void ValidateFrame(RhrReplayFrame frame, int index)
        {
            if (
                !float.IsFinite(frame.PositionX)
                || !float.IsFinite(frame.PositionY)
                || !float.IsFinite(frame.Health)
            )
            {
                throw new ArgumentException($"Frame {index} contains a non-finite value.");
            }
        }
    }
}
