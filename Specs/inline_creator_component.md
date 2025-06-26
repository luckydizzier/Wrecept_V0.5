# Inline Creator Component for Missing Entities

## Context
While entering invoice items users often need to add new products, suppliers or units on the fly. Opening a separate window interrupts the keyboard flow.

## Objectives
- When a lookup field (e.g. Product) does not match any existing record and the user presses Enter, show a small inline form directly below the field.
- The form captures required fields depending on the entity. For products: Termékcsoport, Mértékegység, Egységár, ÁFA.
- Saving instantly adds the entity to the corresponding list and selects it in the editing context.
- Escape cancels the form.

## Constraints
- No modal windows; the inline form appears inside the current row using row details.
- Interaction must remain fully keyboard controllable. Enter commits, Escape aborts.
- Implementation should be generic so other entities can reuse the component with minimal code.
- UI text remains Hungarian.

## Tasks
1. **CodeGen-CSharp** – Create `InlineCreatorViewModel<T>` with `SaveCommand` and `CancelCommand`. Extend `InvoiceItemsViewModel` to trigger `InlineProductCreatorViewModel` when a product name is unknown.
2. **CodeGen-XAML** – Build `InlineProductCreator.xaml` and a reusable `InlineCreatorControl` for other entities. Integrate into `InvoiceItemsGrid.xaml` via `RowDetailsTemplate`.
3. **TestWriter** – Unit tests covering product creation and selection logic.
4. **ux_agent** – Document ghost row visuals and inline form behavior in `docs/ui_flow.md`.
5. **DocWriter** – Update `user_manual.md` with new inline creation steps and mention the component in `architecture.md`.
6. **Architect** – Add TODO and milestone entries for the inline creator feature.
