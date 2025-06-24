# Invoice List Keyboard Behavior

## Context
Users manage invoices using keyboard-first workflows. The invoice list must allow navigation and creation without using the mouse.

## Objectives
- Automatically select the latest invoice on startup.
- Navigate invoices with Up/Down arrows.
- When pressing Up on the top row, ask to create a new invoice:
  - Prompt: "Create new invoice? (I: Yes, N or Esc: No)"
  - On `I` or Enter → open `InvoiceEditorView` in Edit mode with empty data.
  - On `N` or Esc → cancel and keep current selection.
- Press Enter on a selected invoice to open `InvoiceEditorView` in read-only mode.
- Provide initial mock invoices for testing.

## Constraints
- Keyboard interactions must function without mouse input.
- Dialogs are keyboard-only with explicit choices.
- `InvoiceEditorView` differentiates View and Edit modes via a boolean flag.
