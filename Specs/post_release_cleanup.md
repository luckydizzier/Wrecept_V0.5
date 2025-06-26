# Post-Release Cleanup Tasks

## Context
Several components were left in a provisional state during the initial feature cycles. InlineCreator only supports Products, invoice totals lack textual representation and some dialogs are untested.

## Objectives
- Extend the InlineCreator mechanism to allow creating additional master data (first target: Supplier).
- Implement textual amount output for invoice grand totals using a reusable number-to-text converter.
- Finalize placeholder row handling in `InvoiceItemsViewModel`.
- Replace any leftover placeholder service implementations.
- Provide basic logic in invoice part views beyond calling `InitializeComponent`.
- Cover `NavigationService` dialogs with unit tests.

## Constraints
- Maintain keyboard-centric navigation and Hungarian UI text.
- Converter must work offline without external libraries.
- New functionality must be covered by automated tests.

## Tasks
1. **CodeGen-CSharp** – Port `num2hun.py` logic into a `HungarianNumberConverter` utility under `Wrecept.Core.CoreLib` and expose `ToText(decimal)`.
2. **CodeGen-CSharp** – Use the converter in `GrandTotal.AmountText` and add simple initialisation logic to invoice part code-behind files.
3. **CodeGen-CSharp** – Implement `InlineSupplierCreatorViewModel` reusing the generic base.
4. **CodeGen-CSharp** – Ensure `InvoiceItemsViewModel` placeholder row behaves correctly after adding an item.
5. **TestWriter** – Unit tests for the converter and updated ViewModel logic including `NavigationService` interactions.
6. **DocWriter** – Document the number-to-text feature in `architecture.md` and update `user_manual.md`.
7. **Architect** – Update `TODO.md` and `MILESTONES.md` to track these items.
