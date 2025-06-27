# Lookup Autocomplete and Price Memory

## Context
Keyboard-driven lookup dropdowns currently open via F2 or Ctrl+L. This extra keystroke slows down data entry when adding invoice items or selecting suppliers.

## Objectives
- Automatically open the inline lookup list when entering name fields (supplier, product, unit, tax rate).
- Filter results as the user types and allow selection with Up/Down + Enter.
- When a product is chosen, pre-fill the unit price with the most recent price used for that product.
- Keep price history so previous values remain queryable.

## Constraints
- Must remain fully keyboard navigable; Esc closes the list without selection.
- Store price history locally without introducing new external dependencies.
- UI strings remain Hungarian.

## Tasks
1. **CodeGen-XAML** – Add focus handlers to open lookup dropdowns automatically in `InvoiceHeader` and `InvoiceItemsGrid`.
2. **CodeGen-CSharp** – Implement `JsonPriceHistoryService` with `GetLatestPrice` and `RecordPrice` methods.
3. **CodeGen-CSharp** – Inject price service into `AppContext` and record prices when saving invoices.
4. **CodeGen-CSharp** – On product lookup selection, fill unit price from history when available.
5. **DocWriter** – Update `ui_flow.md` and `user_manual.md` to describe automatic lookup behaviour.
6. **TestWriter** – Unit tests for `JsonPriceHistoryService` retrieving latest price and recording new entries.
7. **Architect** – Add TODO and milestone entries for this feature.
