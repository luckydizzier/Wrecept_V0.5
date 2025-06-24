# UI Flow

## Invoice List
- On startup the first invoice is selected automatically.
- Arrow Up/Down moves the selection.
- If the first row is selected and Up is pressed, a confirmation dialog appears asking to create a new invoice.
- Enter on a row opens the invoice editor in read-only mode.
- Confirming creation opens the editor in edit mode with empty data.
- Escape closes the editor or dismisses the confirmation.
- Repeated Up or Down at the edges plays a beep and keeps the current row.
- Deleting a row selects the last remaining invoice if any.
