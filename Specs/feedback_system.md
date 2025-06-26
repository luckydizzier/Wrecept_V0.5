# Unified Feedback System

## Context
Current feedback in Wrecept relies on basic system beeps and static validation colors. The behaviour is inconsistent and provides no visual cues when input is accepted or rejected.

## Objectives
- Short audio patterns indicate startup, exit, confirmation and errors.
- Invalid input flashes the affected field in yellow or red 2-4 times.
- Successful actions briefly turn the field green.
- All cues remain keyboard friendly and non-intrusive.

## Constraints
- Implementation must work offline using built-in .NET APIs.
- No third-party sound libraries.
- Colours integrate with existing theme resources.

## Tasks
1. **CodeGen-CSharp** – implement `IFeedbackService` with methods for startup, exit, accept, reject and error sounds using `Console.Beep`.
2. **CodeGen-CSharp** – add `VisualFeedback` helper for flashing controls in warning, error and success colours.
3. **CodeGen-CSharp** – register the service in `AppContext` and invoke from main navigation flows.
4. **CodeGen-XAML** – extend `TextBox` error style with flashing animation using `FlashWarningColor`.
5. **TestWriter** – verify sound sequences and service invocation.
6. **DocWriter** – describe the new feedback system in `architecture.md`, `ui_flow.md` and `themes.md`.
7. **Architect** – track progress in TODO and `MILESTONES.md` under M7.2.
