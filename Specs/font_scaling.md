# Font Scale Setting

## Context
The UI currently uses fixed font sizes, which may be difficult to read for some users. A configurable scale factor will improve accessibility.

## Objectives
- Add `FontScale` property to `settings.json` with default `0`.
- Base font size is defined as `14 + FontScale` via a dynamic resource.
- Provide slider or numeric input in the Settings window to adjust between `-5` and `+5`.
- Changes take effect immediately without restarting and persist across sessions.

## Constraints
- Layouts must remain usable across the range.
- Keyboard navigation must not be affected.

## Tasks
1. **CodeGen-CSharp** – extend `Settings`, `JsonSettingsService` and `SettingsViewModel` with `FontScale` support.
2. **CodeGen-XAML** – define `BaseFontSize` dynamic resource and UI control for scale plus reset button.
3. **TestWriter** – unit tests covering load and save of the new value.
4. **DocWriter** – update manuals and create settings reference entry.
5. **ux_agent** – update `ui_flow.md` with focus behaviour if needed.
6. **Architect** – mark TODO and milestone entries.

## Implementation Summary
- `App.OnStartup` reads the scale and sets `BaseFontSize` in `Application.Resources`.
- `SettingsViewModel.SaveCommand` writes the updated scale and triggers resource refresh.
- Slider value binding ensures instant feedback.
