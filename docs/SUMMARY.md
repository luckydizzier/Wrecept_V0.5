# Project Summary – Wrecept

## 1. Modules Overview

| Module | Purpose | Layer | Owned by |
|-------|---------|-------|---------|
| `src/ViewModels` | View-model logic, MVVM glue | MVVM | CodeGen-CSharp |
| `src/Views` | XAML views and dialogs | UI | CodeGen-XAML |
| `src/Infrastructure` | EF Core repositories, settings, helpers | Persistence | CodeGen-CSharp |
| `src/Services` | Application services and navigation | Domain/Service | CodeGen-CSharp |
| `src/Themes` | Resource dictionaries for light/dark themes | Styling | CodeGen-XAML |
| `src/Resources` | Localisation strings (hu/en) | Shared | CodeGen-XAML |
| `src/Wrecept.Core.CoreLib` | Entities and repository interfaces | Core Library | CodeGen-CSharp |
| `src/Wrecept.Core` | Default service implementations | Core Services | CodeGen-CSharp |
| `src/Wrecept.Plugin.Greeting` | Sample IMenuPlugin implementation | Plugin | CodeGen-CSharp |
| `tests` | xUnit and UI tests | QA | TestWriter |

## 2. View Hierarchy

```
MainWindow
 └── InvoiceEditorWindow
       ├── InvoiceSidebar
       ├── InvoiceHeader
       ├── InvoiceItemsGrid
       └── InvoiceSummary
```
Additional windows: `SettingsWindow`, `HelpWindow`, `AboutWindow`, lookup dialogs and filter dialogs.

## 3. Dependencies

```mermaid
graph TD
    UI --> ViewModels
    ViewModels --> Services
    Services --> Repositories
    Repositories --> Entities
```

## 4. Keyboard Input Routing

| Key Combo | Function | Handled in |
|-----------|----------|-----------|
| Ctrl+S | Save current invoice | `InvoiceEditorViewModel` |
| Esc | Cancel / exit | `InvoiceEditorViewModel` and dialogs |
| F1 | Help overlay | `HelpWindow` |
| Tab | Field navigation | All views |
| F2 or Ctrl+L | Open lookup dialogs | `InvoiceItemRowViewModel` |

## 5. Task Mapping

| Area | Agent | Reference Files |
|------|-------|----------------|
| New XAML layout | CodeGen-XAML | `src/Views/**`, `docs/themes.md` |
| New ViewModel logic | CodeGen-CSharp | `src/ViewModels/**` |
| UX flow review | ux_agent | `docs/ui_flow.md` |
| Error message updates | DocWriter | `docs/user_manual_hu.md` |
| Keyboard handling | CodeGen-CSharp | `InvoiceEditorWindow.xaml.cs`, `*ViewModel.cs` |

