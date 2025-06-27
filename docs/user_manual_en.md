# User Manual

## Shortcuts
- **Alt+S** – Open Invoices menu
- **Alt+T** – Open Master Data menu
- **Alt+L** – Open Lists menu
- **Alt+H** – Open Help menu
- **F1** – Show keyboard help (Help → Keyboard)
- **PgUp/PgDn** – page through lists

Use the arrow keys and Enter to navigate menu items. The Exit item closes the application.

## Using Filters
Select Date, Supplier, Product Group or Product filters from **Alt+L**. Press Enter to apply, Esc to cancel.

## Master Data Maintenance
In **Alt+T** choose Suppliers or Products. **Insert** adds a new record, **F2** saves changes, **Del** deletes the selected entry.

## Invoice Item Handling
The total amount is also spelled out below the summary using HungarianNumberConverter. The **Add** button (or Enter on the last field) stores the new line. Missing required fields highlight the row in red. If the product name is unknown, pressing Enter shows a small inline form where you can fill in the details. Save (or Enter) immediately adds the product and populates the item; Esc closes the form. When a name field gains focus, a lookup list opens automatically and filters as you type. F2 or Ctrl+L can open it manually as well.

## Help and About
The **Alt+H** menu provides access to **Keyboard** (F1), the user manual and the **About** dialog. All of these windows close with Esc.

## Settings and Themes
Open **Help → Settings** to choose Light or Dark theme and the application language. Settings are automatically saved to `%LOCALAPPDATA%/Wrecept/settings.json` on exit and restored next launch.

## Plugins
Commands from optional plugins appear under **Help → Plugins**. Copy plugin assemblies to a `Plugins` folder beside `Wrecept.exe`. Further info: [plugin_command_bar.md](plugin_command_bar.md).

## Error Handling
If the SQLite database is unavailable, an error message is shown and the application continues in memory. For a locked file, close the other instance. If the database is corrupted, a new empty file is offered and the old one is kept with `.bak` extension. Missing files are created automatically. See `errors.log` under `%LOCALAPPDATA%/Wrecept` for details.
Saving settings or exporting an invoice may fail due to file permissions; such errors are logged to the same file and reported with a friendly message.
