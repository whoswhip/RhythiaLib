namespace RhythiaLib.Rhr.Internal
{
    internal static class RhrFrameCodec
    {
        public const int WireSize = 17; // total size of frame in bytes

        public static RhrReplayFrame Read(BinaryReader reader, int version)
        {
            ArgumentNullException.ThrowIfNull(reader);

            bool usesInt32Time = version >= RhrVersions.Int32Time;
            float? legacyWireTime = usesInt32Time ? null : reader.ReadSingle();
            int time = usesInt32Time
                ? reader.ReadInt32()
                : ConvertLegacyTime(legacyWireTime!.Value);

            float positionX = reader.ReadSingle();
            float positionY = reader.ReadSingle();
            float health = reader.ReadSingle();
            byte isImportantFrame = reader.ReadByte();

            if (version < RhrVersions.NegateY)
                positionY = -positionY;

            return new RhrReplayFrame
            {
                Time = time,
                PositionX = positionX,
                PositionY = positionY,
                Health = health,
                IsImportantFrame = isImportantFrame,
                LegacyWireTime = legacyWireTime,
                DecodedLegacyTime = legacyWireTime.HasValue ? time : null,
            };
        }

        public static void Write(BinaryWriter writer, RhrReplayFrame frame, int version)
        {
            ArgumentNullException.ThrowIfNull(writer);
            ArgumentNullException.ThrowIfNull(frame);

            bool usesInt32Time = version >= RhrVersions.Int32Time;
            bool negateY = version < RhrVersions.NegateY;

            if (usesInt32Time)
                writer.Write(frame.Time);
            else
                writer.Write(
                    frame.LegacyWireTime.HasValue && frame.DecodedLegacyTime == frame.Time
                        ? frame.LegacyWireTime.Value
                        : (float)frame.Time
                );

            writer.Write(frame.PositionX);
            writer.Write(negateY ? -frame.PositionY : frame.PositionY);
            writer.Write(frame.Health);
            writer.Write(frame.IsImportantFrame);
        }

        private static int ConvertLegacyTime(float value)
        {
            if (!float.IsFinite(value))
            {
                throw new InvalidRhrFormatException(
                    "A legacy replay frame contains a non finite time."
                );
            }

            if (value > int.MaxValue || value < int.MinValue)
            {
                throw new InvalidRhrFormatException(
                    $"Legacy frame time {value} is outside the Int32 range."
                );
            }

            return (int)value;
        }
    }
}
