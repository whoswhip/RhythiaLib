namespace RhythiaLib.Rhr.Internal
{
    internal static class RhrScoreDataCodec
    {
        public static RhrScoreData Read(BinaryReader reader, int version)
        {
            ArgumentNullException.ThrowIfNull(reader);

            var scoreData = new RhrScoreData
            {
                TimestampTicks = reader.ReadInt64(),
                PlayerName = reader.ReadString(),
                LegacyMapId = reader.ReadString(),
                MapId = reader.ReadInt32(),
                StartFrom = reader.ReadInt32(),
                Mode = reader.ReadString(),
            };

            if (version >= RhrVersions.ExtendedFields)
            {
                scoreData.Passed = reader.ReadBoolean();
                scoreData.ModsJson = reader.ReadString();
                scoreData.Spin = reader.ReadBoolean();
                scoreData.Speed = reader.ReadSingle();
                scoreData.TotalScore = reader.ReadInt64();
            }
            else
            {
                scoreData.Passed = true;
                scoreData.ModsJson = "[]";
                scoreData.Spin = false;
                scoreData.Speed = 0f;
                scoreData.TotalScore = 0;
            }

            scoreData.Accuracy = reader.ReadSingle();
            scoreData.Hits = reader.ReadInt32();
            scoreData.Misses = reader.ReadInt32();
            scoreData.Points = reader.ReadSingle();

            if (version >= RhrVersions.FailTime)
            {
                scoreData.FailTime = reader.ReadInt32();
                scoreData.Failed = scoreData.FailTime >= 0;
            }
            else
            {
                scoreData.FailTime = -1;
                scoreData.Failed = false;
            }

            scoreData.BeatmapHash =
                version >= RhrVersions.BeatmapHash ? reader.ReadString() : string.Empty;

            return scoreData;
        }

        public static void Write(BinaryWriter writer, RhrScoreData scoreData, int version)
        {
            ArgumentNullException.ThrowIfNull(writer);
            ArgumentNullException.ThrowIfNull(scoreData);

            writer.Write(scoreData.TimestampTicks);
            writer.Write(scoreData.PlayerName);
            writer.Write(scoreData.LegacyMapId);
            writer.Write(scoreData.MapId);
            writer.Write(scoreData.StartFrom);
            writer.Write(scoreData.Mode);

            if (version >= RhrVersions.ExtendedFields)
            {
                writer.Write(scoreData.Passed);
                writer.Write(scoreData.ModsJson);
                writer.Write(scoreData.Spin);
                writer.Write(scoreData.Speed);
                writer.Write(scoreData.TotalScore);
            }

            writer.Write(scoreData.Accuracy);
            writer.Write(scoreData.Hits);
            writer.Write(scoreData.Misses);
            writer.Write(scoreData.Points);

            if (version >= RhrVersions.FailTime)
            {
                writer.Write(scoreData.Failed ? scoreData.FailTime : -1);
            }

            if (version >= RhrVersions.BeatmapHash)
            {
                writer.Write(scoreData.BeatmapHash);
            }
        }
    }
}
