# Next Feature Cycle – Unified Agent Prompt

## Context
Wrecept is evolving from a prototype into a feature-complete, keyboard-driven invoice tool. The upcoming cycle introduces persistent storage, advanced filtering, master-data management and user theme settings.

## Objectives
- Implement date, supplier, product group and product filters accessible from the **Listák** menu.
- Provide CRUD views for suppliers and products under **Törzsek**.
- Replace in-memory repositories with SQLite-based repositories.
- Seed demo data on first run if the database is empty.
- Add a settings window for theme selection and user preferences stored in a JSON file.

## Constraints
- All dialogs and maintenance views must be fully keyboard navigable.
- SQLite file stored under `%LOCALAPPDATA%/Wrecept/wrecept.db`.
- Application should fall back to in-memory repositories when the DB is unavailable.
- Error messages for locked or inaccessible DBs must be clear to the user.

## Tasks
1. **schema_agent** – create `db/schema_v1.sql` defining tables matching domain models.
2. **infra_agent** – implement `Sqlite{Entity}Repository` classes and wire via `AppContext`.
3. **viewmodel_agent** – add filter dialog and master data ViewModels, plus settings ViewModel.
4. **ui_agent** – build corresponding XAML views with keyboard support.
5. **ux_agent** – document focus rules and status messages for filters and settings.
6. **test_agent** – unit tests for repositories and integration tests for filtering.
7. **docs_agent** – update README and user manual with new features, DB location and shortcuts.

## Outcome
At the end of this cycle Wrecept should persist data across restarts, allow quick filtering of invoices and maintain suppliers/products through dedicated views. Themes and settings will be user configurable, completing the initial desktop feature set.
