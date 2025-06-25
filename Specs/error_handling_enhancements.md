# SQLite Error Handling Enhancements

## Context
The application currently falls back to in-memory repositories when the SQLite database cannot be opened. Users receive a generic warning and any persistent data is lost for that session.

## Objectives
- Detect database file locking and show a clear message to close other instances.
- When the database file is corrupt, offer to recreate it after backing up the invalid file.
- Gracefully create a new database if the file is missing.

## Constraints
- The recovery flow must remain keyboard operable.
- Corrupt database recovery simply renames the old file with a `.bak` suffix and creates a new empty database.
- All new dialogs reuse the existing `KeyboardConfirmDialog`.

## Agent Tasks
1. **CodeGen-CSharp** – Extend `AppContext` with recovery helpers and error-code checks. Update `App.xaml.cs` to ask the user on corruption.
2. **TestWriter** – Unit tests covering corrupt file detection and recovery.
3. **DocWriter** – Document the new behaviour in `user_manual.md`.
