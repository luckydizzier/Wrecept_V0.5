# Development Setup

## Required Tools
- .NET SDK 8.0 or newer
- SQLite command-line tools
- Bash shell (for `setup.sh`)

## Initial Setup
1. Clone the repository.
2. Run `./setup.sh` to restore, build and test the solution.
3. A helyi SQLite adatbázis a `%LOCALAPPDATA%/Wrecept/wrecept.db` útvonalon jön létre.

## Troubleshooting
- Ensure `dotnet` is available on the PATH.
- On first run, give execution permission to the script: `chmod +x setup.sh`.
- If tests fail, clear the `bin` and `obj` folders and rerun the script.
- Ha a főablak nem jelenik meg indításkor, töröld a helyi adatbázist vagy futtasd `dotnet run Wrecept.csproj` parancssal, hogy lásd a naplókat.
