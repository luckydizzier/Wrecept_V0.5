# User Interaction Rules

## Edge-of-List Feedback
- When navigation hits the first or last item, the UI plays a short beep and displays a subtle status message.
- The selection never moves outside the list bounds.
- Rapid key presses are debounced so the confirmation dialog cannot open twice.

## Keyboard Discoverability
- On first launch a tooltip overlay highlights the main keys (Up/Down, Enter, Esc).
- Users can reopen the overlay from the Help menu with F1.

## Database Location
- The SQLite file resides under `%LOCALAPPDATA%/Wrecept/wrecept.db` by default.
- `AppContext` exposes a helper to retrieve this path and logs it during startup.
