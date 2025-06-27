# Entity Lookup for Related Names

## Context
Current input fields for referencing related entities (e.g. product name, supplier name, product group name) only accept free text. Selecting an existing entry requires manual typing or navigating separate lists, slowing down data entry.

## Objectives
- Provide an inline dropdown that lists existing entities and allows searching by name.
- Invoke the dropdown automatically when the corresponding field receives focus (Product, Supplier, Product Group, Unit, TaxRate).
- Selection fills the field and keeps focus within the editing context.
- Seamlessly integrate with the inline creator for new entries.

## Constraints
- Keyboard-only navigation: open via `F2` or `Ctrl+L`, navigate with arrows, confirm with `Enter`, cancel with `Esc`.
- Reuse existing services for data retrieval; no additional persistence layer.
- UI text must remain Hungarian.

## Tasks
1. **CodeGen-XAML** – Build `LookupBox` user control with embedded search box and list.
2. **CodeGen-CSharp** – Implement `LookupBoxViewModel` and selection callbacks.
3. **CodeGen-CSharp** – Integrate the control into invoice views and bind keyboard handlers.
4. **ux_agent** – Document focus order and keyboard cues in `docs/ui_flow.md`.
5. **TestWriter** – Add unit tests for lookup selection and cancellation paths.
6. **DocWriter** – Update user manual and architecture diagrams with the new lookup feature.
7. **Architect** – Track progress in `TODO.md` and add milestone entries.
