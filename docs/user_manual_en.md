# User Manual

## Shortcuts
- **Alt+S** – Open Invoices menu
- **Alt+T** – Open Master Data menu
- **Alt+L** – Open Lists menu
- **Alt+H** – Open Help menu
- **F1** – Show keyboard help
- **PgUp/PgDn** – page through lists

Use the arrow keys and Enter to navigate menu items. The Exit item closes the application.

## Using Filters
Select Date, Supplier, Product Group or Product filters from **Alt+L**. Press Enter to apply, Esc to cancel.

## Master Data Maintenance
In **Alt+T** choose Suppliers or Products. **Insert** adds a new record, **F2** saves changes, **Del** deletes the selected entry.

## Settings and Themes
Choose Light or Dark theme in the **Settings** window. Settings are automatically saved to `%LOCALAPPDATA%/Wrecept/settings.json` on exit and restored next launch.

## Error Handling
If the SQLite database is unavailable, an error message is shown and the application continues in memory. For a locked file, close the other instance. If the database is corrupted, a new empty file is offered and the old one is kept with `.bak` extension. Missing files are created automatically. See `errors.log` under `%LOCALAPPDATA%/Wrecept` for details.
