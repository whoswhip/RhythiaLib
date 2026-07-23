## Basic usage

### Read an RHR file

```csharp
using RhythiaLib.Rhr;

RhrReplay replay = RhrFile.Read("replay.rhr");

Console.WriteLine($"Version: {replay.Version}");
Console.WriteLine($"Player: {replay.ScoreData.PlayerName}");
Console.WriteLine($"Map: {replay.ScoreData.LegacyMapId}");
Console.WriteLine($"Accuracy: {replay.ScoreData.Accuracy}%");
Console.WriteLine($"Score: {replay.ScoreData.TotalScore}");
Console.WriteLine($"Frames: {replay.Frames.Count}");
```

### Read from a stream

```csharp
using RhythiaLib.Rhr;

using var stream = File.OpenRead("replay.rhr");
RhrReplay replay = RhrFile.Read(stream);
```

### Decode from bytes


```csharp
using RhythiaLib.Rhr;

byte[] data = File.ReadAllBytes("score-replay.rhr");
RhrReplay replay = RhrFile.Decode(data);
```

### Write an RHR file

```csharp
using RhythiaLib.Rhr;

RhrReplay replay = RhrFile.Read("replay.rhr");
replay.ScoreData.PlayerName = "NewPlayerName";
RhrFile.Write("new-replay.rhr", replay);
```

### Encode to bytes

```csharp
using RhythiaLib.Rhr;

RhrReplay replay = RhrFile.Read("replay.rhr");
byte[] data = RhrFile.Encode(replay);
File.WriteAllBytes("new-replay.rhr", data);
```