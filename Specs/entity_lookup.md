# Entity Lookup for Related Names

## Context
Current input fields for referencing related entities (e.g. product name, supplier name, product group name) only accept free text. Selecting an existing entry requires manual typing or navigating separate lists, slowing down data entry.

## Objectives
- Provide a lookup dialog that lists existing entities and allows searching by name.
- Invoke the lookup from any name field such as Product, Supplier, Product Group and Unit.
- Selection fills the field and keeps focus within the editing context.
- Seamlessly integrate with the inline creator for new entries.

## Constraints
- Keyboard-only navigation: open via `F3`, navigate with arrows, confirm with `Enter`, cancel with `Esc`.
- Reuse existing services for data retrieval; no additional persistence layer.
- UI text must remain Hungarian.

## Tasks
1. **CodeGen-XAML** – Build `LookupDialog.xaml` with search box and list control.
2. **CodeGen-CSharp** – Implement `LookupDialogViewModel` and integrate with `NavigationService`.
3. **CodeGen-CSharp** – Add commands in related ViewModels to trigger the dialog and assign the selected entity.
4. **ux_agent** – Document focus order and keyboard cues in `docs/ui_flow.md`.
5. **TestWriter** – Add unit tests for lookup selection and cancellation paths.
6. **DocWriter** – Update user manual and architecture diagrams with the new lookup feature.
7. **Architect** – Track progress in `TODO.md` and add milestone entries.
