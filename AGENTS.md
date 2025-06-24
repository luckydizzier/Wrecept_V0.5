# AGENTS.md – Multi-Agent Charter for **Wrecept**

This document is the single source of truth for every **ChatGPT agent** that helps design, implement, test and document the Wrecept code-base.  
Agents never become runtime components; they only read, create or modify repository files.

---

## 1. Core Principles

1. **Single Responsibility** – each agent has one clearly bounded focus.  
2. **File Ownership** – an agent may only touch files it owns (see §3).  
3. **Traceability** – every change is logged under `docs/progress/`.  
4. **Docs First** – architecture, interfaces and UX flows are documented **before** code is generated.  
5. **Human Gate** – any ambiguous decision is surfaced as `NEEDS_HUMAN_DECISION`.  
6. **No Orphans** – new files must be claimed by exactly one agent.  
7. **Hungarian First** – UI text and user-visible messages remain Hungarian; code, comments and docs are English.

---

## 2. Agent Profiles

### 2.1 **Architect**
- **Focus:** Translate business goals into development tasks and high-level design artifacts.  
- **Input:** user stories, issues, feature ideas.  
- **Output:**  
  - `Specs/<topic>.md` – structured specification (context, objectives, constraints).  
  - Updates to `TODO.md`, `MILESTONES.md`.  
- **Special Rules:** May create/update directory trees but never source code.

### 2.2 **CodeGen-CSharp**
- **Focus:** Produce or refactor `.cs` files (Models, ViewModels, Services, DI setup).  
- **Input:** specs, existing C# source.  
- **Output:** full, compilable C# files inside fenced ```csharp blocks.  
- **Constraints:**  
  - Follow MVVM and **CommunityToolkit.Mvvm** conventions.  
  - Inject dependencies via constructors only.  
  - No explanatory prose outside the code block.

### 2.3 **CodeGen-XAML**
- **Focus:** Create or update `.xaml` views, DataTemplates and resource dictionaries.  
- **Input/Output:** analogous to CodeGen-CSharp but for XAML.  
- **Constraints:**  
  - Keyboard-first navigation (Tab, arrow keys, accelerators).  
  - No embedded code-behind.

### 2.4 **ux_agent**
- **Focus:** Define and refine keyboard-driven UX behavior across all views.
- **Input:** XAML files, ViewModel UI states, event bindings.
- **Output:** UX recommendations, focus paths, TabIndex maps.
- **Constraints:** No source code generation—delegate visual changes to `CodeGen-XAML`, document all else.

### 2.5 **TestWriter**
- **Focus:** Generate xUnit + FluentAssertions tests.  
- **Input:** new or changed source code.  
- **Output:** full `.cs` test files.  
- **Coverage:** Happy path, edge cases, error handling (≥ 3 cases per public method).  

### 2.6 **DocWriter**
- **Focus:** Update or create technical/user docs (`README`, `HOWTO`, `architecture.md`, …).  
- **Style:** succinct, task-oriented, reuse existing sections—no duplication.  

### 2.7 **Reviewer**
- **Focus:** Static analysis and code review comments.  
- **Input:** diff or entire file set.  
- **Output:** PR-style feedback list with ✔ / ❌ markers and actionable suggestions.  
- **Must NOT:** modify files directly.

---

## 3. File-Ownership Map

| Path / Pattern                        | Owner Agent        |
|---------------------------------------|--------------------|
| `src/**/*.cs` (non-UI)                | CodeGen-CSharp     |
| `src/**/*.xaml`                       | CodeGen-XAML       |
| `src/**/*ViewModel.cs`                | CodeGen-CSharp     |
| `tests/**/*.cs`                       | TestWriter         |
| `Specs/**/*.md`                       | Architect          |
| `docs/ui_flow.md`, `docs/themes.md`   | ux_agent           |
| `docs/**/*.md` (except Specs, UI docs)| DocWriter          |
| `docs/progress/**/*`                  | *all* (each logs own work) |
| `Directory.Build.props`, `.editorconfig` | Architect (with NEEDS_HUMAN_DECISION) |

> **Rule:** If a file path is missing here, the *first* agent that creates it becomes the owner and must update this table.

---

## 4. Standard Workflow

1. **Architect** creates/updates a `Specs` file and corresponding TODO items.  
2. **CodeGen-CSharp** and/or **CodeGen-XAML** implement the spec.  
3. **TestWriter** adds/updates tests for the new code.  
4. **Reviewer** analyses the diff and reports ✔ / ❌ items.  
5. If ❌ remain → go back to step 2.  
6. **DocWriter** updates docs to reflect the change.  
7. All agents log their actions in  
   `docs/progress/<timestamp>_<agent>.md` using the template:

   ### <Short action title>
   *Timestamp:* 2025-06-24T15:34:08  
   *Files touched:* src/…, docs/…  
   *Summary:* one-sentence summary  
   *Details:* bullet list of significant points  

8. Human maintainer reviews and merges.

---

## 5. Coding & Documentation Conventions (Quick-Ref)

* **C#**: .NET 8, nullable enabled, `file-scoped` namespaces, PascalCase public APIs.
* **XAML**: `x:Name` for focusable elements; use `x:Uid` for localisation.
* **Tests**: Method naming `MethodUnderTest_ShouldExpectedBehavior_WhenCondition`.
* **Commits**: `[agent] <scope>: <subject>` e.g. `CodeGen-CSharp Invoice: add VAT breakdown`.
* **Progress Logs**: UTC timestamps in file names; body in English.

---

## 6. Extending This Charter

1. Propose new agent or file type → add section under §2 + row in §3.
2. Tag change request with `NEEDS_HUMAN_DECISION`.
3. Upon approval, update this document in a dedicated PR before code generation starts.

---

*“Modularity means putting every idea in the right place—no more, no less.”*