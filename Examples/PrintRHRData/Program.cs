using RhythiaLib.Rhr;

namespace PrintRHRData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No path provided");
                return;
            }
            string path = args[0];
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                Console.WriteLine("File not found");
                return;
            }

            RhrReplay replay = RhrFile.Read(path);
            Console.WriteLine($"Version: {replay.Version}");
            Console.WriteLine($"Player: {replay.ScoreData.PlayerName}");
            Console.WriteLine($"Map: {replay.ScoreData.LegacyMapId}");
            Console.WriteLine($"Accuracy: {replay.ScoreData.Accuracy}%");
            Console.WriteLine($"Misses: {replay.ScoreData.Misses}");
            Console.WriteLine($"Score: {replay.ScoreData.TotalScore}");
            Console.WriteLine($"Frames: {replay.Frames.Count}");
        }
    }
}
