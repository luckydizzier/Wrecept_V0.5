## ✨ Project Overview

**Wrecept** is a modern WPF desktop application built in C# and optimized for rapid, keyboard-centric invoice entry workflows.  
It is designed for offline use by individual operators such as small business owners, chefs, or workshop managers who need a fast, local, and reliable tool without cloud dependencies.

This project is a modern reimplementation of an earlier DOS/DBASE 4/Clipper written (RECEPT), now using:

- MVVM architecture with clear separation of logic and UI
- SQLite database storage with local .db file
- Modular service-agent coordination
- Optional plugins loaded from the `Plugins` folder
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
- Settings mentés a %AppData%/Wrecept/settings.json fájlba
- Planned: printing, VAT declaration generation

### Gyorsbillentyűk
- `Ctrl+S` – mentés és visszatérés a listához
- `Esc` – menüsor fókusz / vissza a listára

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
- **Central `AppContext` bootstraps services with in-memory repositories**
- **Theme + i18n ready** via resource dictionaries

---

## 🧑‍💻 For Developers

### Prerequisites

- Windows 10 (Parallels or native)
- Visual Studio 2022 (Community or higher)
- .NET SDK 8.0+
- Git for Windows
- Download `Wrecept-win-x64.exe` from Releases and run

### Installation

```bash
./publish.sh
iscc installer.iss
```

The `publish/` folder will contain the self-contained `Wrecept.exe` and supporting files. The installer output `Output/WreceptInstaller.exe` can be distributed to end users.
Plugins should be copied to a `Plugins` directory beside `Wrecept.exe`.


### Clone and Run

```bash
git clone https://github.com/luckydizzier/wrecept.git
cd wrecept
./setup.sh
```

The executable will be created at `bin/Release/net8.0-windows/win-x64/publish/Wrecept.exe`.

### Testing

Use `./setup.sh` to build and run tests. Manual execution is possible via:

```bash
dotnet test Wrecept.CoreOnly.sln
```

See [docs/dev_setup.md](docs/dev_setup.md) for full details.

Felhasználói útmutató: [docs/user_manual.md](docs/user_manual.md)

Licensed under the MIT License.
