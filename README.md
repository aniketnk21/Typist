# Typist

Typist is a desktop typing tutorial application targeting .NET 8. It provides lessons and exercises to help users improve typing speed and accuracy.

## Key Features

- Interactive typing lessons
- Real-time accuracy and speed metrics
- Progress tracking across lessons

## Requirements

- .NET 8 SDK (https://dotnet.microsoft.com)
- Windows / macOS / Linux (desktop support depends on UI framework used in `Typist.Desktop`)

## Build and run (CLI)

1. Open a terminal in the repository root.
2. Build the desktop project:

```
dotnet build ./Typist.Desktop/Typist.Desktop.csproj -c Release
```

3. Run the application:

```
dotnet run --project ./Typist.Desktop/Typist.Desktop.csproj
```

## Run in Visual Studio / Rider

1. Open the solution containing `Typist.Desktop`.
2. Set `Typist.Desktop` as the startup project.
3. Run (F5) or Debug -> Start Debugging.

## Project structure

- `Typist.Desktop/` — the desktop application project (UI, lessons, user flows).

If you need to add new features, look for UI components and lesson data inside `Typist.Desktop`.

## Contributing

Contributions are welcome. Please open issues or pull requests with a clear description of changes and any required steps to test.

## Troubleshooting

- If build fails, ensure the .NET 8 SDK is installed and `dotnet --info` shows the correct SDK.
- If UI assets or resources are missing, check the `Typist.Desktop` project file for linked files and resource paths.

## License

If this repository does not include a `LICENSE` file, add one to clarify licensing. Otherwise refer to the existing `LICENSE`.

---

If you want, I can add a short developer guide, sample lessons, or CI steps next.
