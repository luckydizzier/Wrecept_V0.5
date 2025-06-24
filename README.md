## ✨ Project Overview

**Wrecept** is a modern WPF desktop application built in C# and optimized for rapid, keyboard-centric invoice entry workflows.  
It is designed for offline use by individual operators such as small business owners, chefs, or workshop managers who need a fast, local, and reliable tool without cloud dependencies.

This project is a modern reimplementation of an earlier DOS/DBASE 4/Clipper written (RECEPT), now using:

- MVVM architecture with clear separation of logic and UI
- SQLite database storage with local .db file
- Modular service-agent coordination
- Fully keyboard-navigable interface
- Export capabilities for PDF, CSV, and Excel

---

## ⚙ Tech Stack

| Layer        | Technology                      |
| ------------ | ------------------------------- |
| Language     | C# (.NET 8)                     |
| UI           | WPF (Windows Presentation Foundation) |
| Architecture | MVVM                            |
| Database     | SQLite + Dapper ORM             |
| Build        | Visual Studio 2022              |
| Packaging    | dotnet publish with single .exe |
| Repo         | GitHub (private)                |

---

## 🌐 Features

- Offline-first, local-only execution
- Fully keyboard-driven operation (F2, F5, F6, etc.)
- Rapid invoice entry with dynamic row addition
- Searchable and filterable invoice list
- VAT breakdown by percentage and totalization
- Configurable themes and defaults
- Export to PDF, XLSX, and CSV
- Local supplier/product master data
- Validation and confirmation dialogs
- Light/dark theme support
- Planned: printing, VAT declaration generation

---

## 🚧 Planned Features

- Dynamic filtering of suppliers/products
- Custom invoice templates (print layout engine)
- Keyboard macro sequences for power users
- Advanced validation framework
- Plugin-based agent extension system

---

## 🧠 Architecture Principles

- **MVVM** separation with reactive UI bindings
- **Agent-based task delegation** for modularity
- **Loose coupling** between UI, services, and persistence
- **Single responsibility per ViewModel and service**
- **Constructor-based dependency injection**
- **Theme + i18n ready** via resource dictionaries

---

## 🧑‍💻 For Developers

### Prerequisites

- Windows 10 (Parallels or native)
- Visual Studio 2022 (Community or higher)
- .NET SDK 8.0+
- Git for Windows

### Clone and Run

```bash
git clone https://github.com/luckydizzier/wrecept.git
cd wrecept
Open the solution file in Visual Studio:
Wrecept.sln
Hit F5 to build and run.
Build Single Executable
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
The output .exe can run independently of .NET installation.
💼 Directory Structure
wrecept/
├── Wrecept.sln
├── Models/              # Invoice, Item, Supplier, TaxRate, etc.
├── ViewModels/          # MVVM logic per screen
├── Views/               # XAML views and user controls
├── Services/            # Business logic (InvoiceService, ExportService)
├── Data/                # SQLite access via Dapper
├── Resources/           # Themes, icons, localization
├── Agents/              # AGENTS.md, task coordination
├── docs/                # Documentation, changelogs
└── TODO.md              # Task tracking
🖱 Keyboard Shortcuts
Navigate fields	ESC, ENTER
📆 Project Status
Area	Status
Project setup	✅ Completed
GitHub repo	✅ Linked
README.md	✅ Done
UI prototype	⏳ In progress
Agents design	🔜 Next step
🗂 Progress Logs
Agent activities and milestone logs are maintained in:
docs/progress/
Each file is timestamped and agent-tagged.
📜 License
MIT License (to be finalized)
“If you're typing, you're working.” — The Keyboard Master
