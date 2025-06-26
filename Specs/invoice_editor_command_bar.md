# Invoice Editor Command Bar

## Context
The current InvoiceEditorWindow lacks visible actions for saving or closing invoices, confusing users. They can't tell when changes are saved, nor can they easily return to other features since the menu bar disappears.

## Objectives
- Introduce a command bar with buttons: **Mentés**, **Nyomtatás**, **Bezárás**, **Export (CSV, Excel, PDF)**.
- Restore the main menu within the editor so users can access other modules.
- Provide a visual "Tétel hozzáadása" button next to the editable row; Enter still works.
- When navigation moves above the first invoice in the list, prompt "Új számlát szeretne kezdeni?".
- Ensure Tab order covers the new command bar elements before the sidebar and header.

## Constraints
- Buttons must have keyboard accelerators (`Alt+M` for Mentés, etc.) and be reachable via Tab.
- ViewModel exposes `SaveCommand`, `PrintCommand`, `CloseCommand` and `ExportCommand`. Export opens a submenu for CSV, Excel, PDF.
- No code-behind for button logic; everything bound to commands.

## Agent Tasks
1. **Architect** – update TODO and milestone entries.
2. **CodeGen-CSharp** – extend `InvoiceEditorViewModel` with new commands and export logic placeholder.
3. **CodeGen-XAML** – design `MenuBar` or bottom `ToolBar` containing the buttons and hook up bindings. Reintroduce the main menu as in MainWindow.
4. **ux_agent** – document the updated focus order and prompts in `docs/ui_flow.md`.
5. **TestWriter** – unit tests ensuring commands flag exit state and save invocation.
6. **DocWriter** – update user manual sections about invoice editing.
