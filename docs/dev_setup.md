# Development Setup

## Required Tools
- .NET SDK 8.0 or newer
- SQLite command-line tools
- Bash shell (for `setup.sh`)

## Initial Setup
1. Clone the repository.
2. Run `./setup.sh` to restore, build and test the solution.
3. A helyi SQLite adatbázis a `%LOCALAPPDATA%/Wrecept/wrecept.db` útvonalon jön létre.
   A létrehozáshoz szükséges `schema_v1.sql` az alkalmazásba van beágyazva, így
   külön fájl másolása nem szükséges.

## Troubleshooting
- Ensure `dotnet` is available on the PATH.
- On first run, give execution permission to the script: `chmod +x setup.sh`.
- If tests fail, clear the `bin` and `obj` folders and rerun the script.
- Ha a főablak nem jelenik meg indításkor, futtasd `dotnet run Wrecept.csproj` parancsot.
  A hibás `settings.json` fájl automatikusan ignorálásra kerül, az adatbázis törlése többnyire nem szükséges.
- Indítási hiba esetén a program `errors.log` fájlba naplóz a `%LOCALAPPDATA%/Wrecept` mappában, 
  és memóriában folytatja a működést.
