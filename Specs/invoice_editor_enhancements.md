# Invoice Editor UX Enhancements

## Context
The current `InvoiceEditorWindow` lacks clear feedback when editing or adding invoice items.
Main menu options and functional buttons are missing, leading to uncertainty
whether changes are saved or how to access common actions.

## Objectives
- Provide visible Save/Close controls and export options.
- Clarify item entry with an explicit Add button and validation feedback.
- Restore the main menu for navigation and access to other modules.
- Confirm new invoice creation when moving above the first list row.

## Constraints
- All interactions must be keyboard accessible.
- Existing MVVM structure and dialog service are reused.
- UI text remains Hungarian.

## Tasks
1. **CodeGen-XAML** – Add a menu bar and bottom toolbar to `InvoiceEditorWindow` with buttons: Mentés, Nyomtatás, Bezárás, Export.
2. **CodeGen-CSharp** – Extend `InvoiceItemsViewModel` with validation and expose an `AddItemCommand` bound to the ➕ button.
3. **CodeGen-CSharp** – Enhance `InvoiceSidebarViewModel` navigation so Up above the first row triggers a confirmation dialog.
4. **ux_agent** – Update `docs/ui_flow.md` describing the new toolbar layout and focus order.
5. **TestWriter** – Cover new ViewModel logic for item addition and navigation confirmation.
6. **DocWriter** – Document toolbar usage and menu recovery in `user_manual.md`.
7. **Architect** – Mark TODO item for invoice editor redesign as completed and add follow-up tasks for export implementation.
