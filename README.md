# RhythiaLib

A .NET 10 C# library for reading and writing Rhythia replay and map files.

Currently only `.rhr` replay files are supported, but `.rhm` map files are easy to implement and will be implemented in the coming days.

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

| Extension | Status    | Description                 |
| --------- | --------- | --------------------------- |
| .rhr      | Supported | Rhythia replay file         |
| .rhm      | Planned   | Rhythia map file/archive    |
| .sspm     | Possibly  | Legacy/SoundSpace+ map file |

## Usage

- [RHR Files](./RhythiaLib/Rhr/USAGE.MD)

## Testing

Run all tests with:

```bash
dotnet test
```

## License
Licensed under the MIT License. See [LICENSE](./LICENSE) for details.