# Development Setup

## Required Tools
- Windows 10 or later (build and tests require the Windows Desktop SDK)
- .NET SDK 8.0 or newer
- SQLite command-line tools
- Bash shell (for `setup.sh`)

## Initial Setup
1. Clone the repository.
2. Run `./setup.sh` to restore, build and test the solution.
3. The local SQLite database is created at `%LOCALAPPDATA%/Wrecept/wrecept.db`.
   If `%LOCALAPPDATA%` is unavailable, the executable directory is used instead.
   The required `schema_v1.sql` is embedded so no extra file is needed.

## Troubleshooting
- Ensure `dotnet` is available on the PATH.
- On first run, give execution permission to the script: `chmod +x setup.sh`.
- If tests fail, clear the `bin` and `obj` folders and rerun the script.
- If the main window doesn't appear on startup, check `errors.log` under
  `%LOCALAPPDATA%/Wrecept`. An invalid `settings.json` is ignored automatically;
  deleting the database is rarely necessary.
- On startup errors the application logs to `errors.log` in `%LOCALAPPDATA%/Wrecept`
  and continues running in memory.
