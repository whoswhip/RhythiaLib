# RhythiaLib

A .NET 10 C# library for reading and writing Rhythia replay and map files.

This library is designed for the Steam version of Rhythia, but it may work with the open-source one as well if they end up using the same formats. As of now (July 2026) the open-source version supports `.rhm` maps.

## Installation

Install the package from Nuget:

```bash
dotnet add package RhythiaLib
```

Or reference the project directly:

```bash
dotnet add reference path/to/RhythiaLib/RhythiaLib/RhythiaLib.csproj
```

## Supported Formats

| Extension  | Status    | Description                    |
| ---------- | --------- | ------------------------------ |
| .rhr       | Supported | Steam Rhythia replay file      |
| .rhm       | Supported | Steam Rhythia map file/archive |
| .sspm      | Planned   | SoundSpace+ map file           |
| .phxm      | Planned   | FOSS Rhythia map file          |
| .sspre     | Planned   | SoundSpace+ replay file        |
| .phxr      | Planned   | FOSS Rhythia replay file       |

## Usage

- [RHR Files](./RhythiaLib/Rhr/USAGE.MD)
- [RHM Files](./RhythiaLib/Rhm/USAGE.MD)

## Testing

Run all tests with:

```bash
dotnet test
```

## Credits

- [yoru](https://github.com/yo-ru) - Their [rhrParse](https://github.com/yo-ru/rhrParse) repo was used as reference for unknown fields and backwards compatibility.
- [FOSS Rhythia](https://github.com/Rhythia/Client) - Their map [parser](https://github.com/Rhythia/Client/blob/d931b419853387ef6d2ecd1b378e409f6de6f630/scripts/map/MapParser.cs)/[model](https://github.com/Rhythia/Client/blob/d931b419853387ef6d2ecd1b378e409f6de6f630/scripts/map/Map.cs) was used as reference.

## License

Licensed under the MIT License. See [LICENSE](./LICENSE) for details.
