## Basic usage

### Read an RHM file

```csharp
using RhythiaLib.Maps;
using RhythiaLib.Rhm;

RhythiaMap map = RhmFile.Read("map.rhm");

Console.WriteLine($"Artist: {map.Artist}");
Console.WriteLine($"Title: {map.Title}");
Console.WriteLine($"Difficulty: {map.Difficulty}");
Console.WriteLine($"Rating: {map.Rating}");
Console.WriteLine($"Length: {map.LengthMilliseconds}ms");
Console.WriteLine($"Notes: {map.Notes.Count}");
```

### Read from a stream

```csharp
using RhythiaLib.Maps;
using RhythiaLib.Rhm;

using var stream = File.OpenRead("map.rhm");
RhythiaMap map = RhmFile.Read(stream);
```

### Decode from bytes

```csharp
using RhythiaLib.Maps;
using RhythiaLib.Rhm;

byte[] data = File.ReadAllBytes("map.rhm");
RhythiaMap map = RhmFile.Decode(data);
```

### Write an RHM file

```csharp
using RhythiaLib.Maps;
using RhythiaLib.Rhm;

RhythiaMap map = RhmFile.Read("map.rhm");
map.Title = "New Title";
RhmFile.Write("new-map.rhm", map);
```

### Encode to bytes

```csharp
using RhythiaLib.Maps;
using RhythiaLib.Rhm;

RhythiaMap map = RhmFile.Read("map.rhm");
byte[] data = RhmFile.Encode(map);
File.WriteAllBytes("new-map.rhm", data);
```
