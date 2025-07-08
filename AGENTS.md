# AGENTS.md (Optimized for Codex)

**Project:** Wrecept  
**Purpose:** Defines modular agent-based dev workflow  
**Maintainer:** root_agent  

---

## Core Principle
Each agent owns a functional domain. All inputs, outputs, dependencies, and constraints must be explicit. Agents must avoid cross-domain logic.

---

## Agent Roles

### 🔧 root_agent
- **Role:** Orchestrates agents, maintains structure
- **Input:** docs/README.md, AGENTS.md
- **Output:** Audits, PR decisions
- **Rule:** Cannot modify business logic

### 🎨 ui_agent/ux_agent
- **Role:** XAML UI (Views/, DataTemplates)
- **Input:** ViewModel structure, Themes
- **Rule:** No storage/service access

### 🧠 logic_agent
- **Role:** Key input, modal behavior
- **Input:** ViewModels, controls
- **Output:** Navigation/event logic
- **Rule:** No persistence

### 🧱 core_agent
- **Role:** Domain models, validation, contracts
- **Output:** Invoice, Product, Supplier, Calculator
- **Rule:** No direct DB access

### 🧑‍💻 code_agent
- **Role:** ViewModels, INotifyPropertyChanged, Commands
- **Input:** UI + Domain requirements
- **Rule:** No logic modification without consensus

### 📦 storage_agent
- **Role:** Persistence (EF Core/SQLite)
- **Output:** Migrations, DbContext, Seed
- **Rule:** Only expose via interface; no logic validation

### 🔊 feedback_agent
- **Role:** Visual/audio feedback (StatusBar, SFX)
- **Rule:** No impact on control flow or storage

### 📝 docs_agent
- **Role:** Document architecture, progress, standards
- **Input:** Commits, milestones
- **Output:** Markdown docs
- **Rule:** No duplication

---

## Execution Flow Samples

**New Entity:**  
`core_agent → storage_agent → code_agent → ui_agent → logic_agent → root_agent`

**New UI Interaction:**  
`ui_agent → logic_agent → feedback_agent → core_agent`

---

## Violations
- `code_agent` must not touch core logic
- `ui_agent` must not access services or DB
- `storage_agent` must not validate domain logic

---

## Recommended Project Structure

```
Wrecept/
│
├── Wrecept.Host/              # WPF launcher, plugin loader
│   ├── App.xaml, App.xaml.cs
│   ├── Bootstrap/
│   └── PluginLoader.cs
│
├── Wrecept.Shared/            # Cross-cutting interfaces and types
│   ├── IPlugin.cs
│   ├── IViewProvider.cs
│   ├── IRepository{T}.cs
│   └── Contracts/
│
├── Wrecept.Plugins/           # Individual functional plugins
│   ├── UI.MainMenu/
│   ├── UI.StatusBar/
│   ├── Core.Invoices/
│   ├── Feature.Export.Pdf/
│   └── ...
│
├── Plugins/                   # Compiled DLLs loaded at runtime
│
├── Wrecept.Tests/             # Integration and component tests
│   └── PluginIntegrationTests/
│
└── docs/                      # AGENTS.md, ARCHITECTURE.md, etc.
```

---

## Last Checkpoints

---

_Last updated: 2025-07-08 19:30:00 by root_agent_
