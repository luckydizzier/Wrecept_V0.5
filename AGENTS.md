AGENTS.md – Multi-Agent Charter for Wrecept

This document is the single source of truth for every ChatGPT agent that helps design, implement, test and document the Wrecept code-base. Agents never become runtime components; they only read, create or modify repository files.

## 1. Core Principles

* **Single Responsibility** – Each agent has one clearly bounded focus.
* **File Ownership** – An agent may only touch files it owns (see §3).
* **Traceability** – Every change is logged under `docs/progress/`.
* **Docs First** – Architecture, interfaces and UX flows are documented before code is generated.
* **Human Gate** – Any ambiguous decision is surfaced as `NEEDS_HUMAN_DECISION`.
* **No Orphans** – New files must be claimed by exactly one agent.
* **Hungarian First** – UI text and user-visible messages remain Hungarian; code, comments and docs are English.
* **Milestone Alignment** – All tasks must reference a milestone and agent in their declaration (see `TASKLOG.md`).
* **Review Cycle** – All changes must pass through Reviewer agent approval.

## 2. Agent Profiles

### 2.1 Architect

* **Focus:** Translate business goals into milestones and specifications.
* **Inputs:** User stories, issues, feature ideas.
* **Outputs:**

  * `Specs/<topic>.md` – Structured specifications.
  * Updates to `MILESTONES.md`, `TODO.md`, `TASKLOG.md`.
  * `Directory.Build.props`, `.editorconfig`, `.csproj`, `.sh`, `.iss` files.
* **Rules:** Does not modify source code directly. May request `NEEDS_HUMAN_DECISION`.

### 2.2 CodeGen-CSharp

* **Focus:** Create or refactor `.cs` files (models, services, ViewModels).
* **Constraints:**

  * MVVM with CommunityToolkit.Mvvm.
  * Dependency injection only via constructor.
  * Output only fenced `csharp` code blocks.
  * Never modify files not explicitly assigned to this agent.

### 2.3 CodeGen-XAML

* **Focus:** Create or update `.xaml` views, DataTemplates, resource dictionaries.
* **Constraints:**

  * Keyboard-first navigation (Tab, arrows, accelerators).
  * No embedded code-behind logic.

### 2.4 ux\_agent

* **Focus:** Define UX behavior and keyboard interaction across views.
* **Outputs:**

  * `docs/ui_flow.md`, focus order maps, recommendations.
* **Constraints:**

  * Does not generate code.
  * Delegates visual/UI changes to CodeGen-XAML.

### 2.5 TestWriter

* **Focus:** Generate xUnit + FluentAssertions test files.
* **Coverage:**

  * Happy path
  * Edge cases
  * Error handling (≥ 3 test cases/public method)

### 2.6 DocWriter

* **Focus:** Maintain project documentation.
* **Outputs:**

  * `README.md`, `HOWTO.md`, `architecture.md`, etc.
* **Style:**

  * Succinct, non-redundant, section-reuse oriented.

### 2.7 Reviewer

* **Focus:** Perform static analysis and code reviews.
* **Outputs:**

  * PR-style feedback list (`✔` / `❌` and actionable comments).
* **Rules:** May not modify files directly.

### 2.8 Maintainer

* **Focus:** Cross-check milestone, TODO and progress logs for consistency.
* **Outputs:**

  * Audit reports, sync proposals.
  * Flags inconsistencies in `TASKLOG.md`.
* **Privileges:**

  * May request automated script adjustments.
  * Coordinates review cycles and merge readiness.

### 2.9 audit_agent

* **Focus:** Identify outdated keyboard shortcuts and verify focus behavior across views.
* **Outputs:**
  * `audit_agent.md` – Operational checklist.
  * Progress logs under `docs/progress/` detailing issues and fixes.
* **Rules:**
  * May annotate code with `DEPRECATED` comments but defers refactoring to owning agents.
  * Runs before major feature merges and every two hours when idle.

## 3. File Ownership Map

| Path / Pattern                      | Owner Agent       |
| ----------------------------------- | ----------------- |
| `src/**/*.cs` (non-UI)              | CodeGen-CSharp    |
| `src/**/*.xaml`                     | CodeGen-XAML      |
| `src/**/*ViewModel.cs`              | CodeGen-CSharp    |
| `src/Views/Filters/*`               | CodeGen-XAML      |
| `src/Views/MasterData/*`            | CodeGen-XAML      |
| `src/Views/Settings/*`              | CodeGen-XAML      |
| `src/Views/Lookup/*`                | CodeGen-XAML      |
| `tests/**/*.cs`                     | TestWriter        |
| `Specs/**/*.md`                     | Architect         |
| `docs/ui_flow.md`, `docs/themes.md` | ux\_agent         |
| `docs/**/*.md` (except Specs, UI)   | DocWriter         |
| `audit_agent.md`                    | DocWriter         |
| `docs/progress/**/*`                | All (self-logged) |
| `*.sh`, `*.iss`, `*.csproj`         | Architect         |
| `Directory.Build.props`             | Architect         |
| `.editorconfig`                     | Architect         |

**Rule:** If a path is missing, the first agent creating it becomes owner and must update this table.

## 4. Standard Workflow

1. **Planning**

   * Architect creates/upgrades milestone entries and specs.
   * Maintainer logs and aligns new tasks in `TASKLOG.md`.

2. **Implementation**

   * CodeGen agents implement features from spec.
   * TestWriter generates tests.
   * ux\_agent proposes or adjusts UX flows.

3. **Review and Merge**

   * Reviewer evaluates diffs and issues ✔ / ❌ decisions.
   * Maintainer coordinates merge readiness.

4. **Documentation**

   * DocWriter updates relevant user/tech docs.
   * All agents log work in `docs/progress/<timestamp>_<agent>.md`:

```markdown
Timestamp: 2025-06-24T15:34:08
Files touched: src/…, docs/…
Summary: One-sentence summary
Details:
- Bullet list of relevant changes or notes
```

## 5. Coding & Documentation Conventions (Quick Ref)

* **C#:** .NET 8, nullable enabled, file-scoped namespaces, PascalCase.
* **XAML:** `x:Name` for focusable elements, `x:Uid` for localisation.
* **Tests:** `MethodUnderTest_ShouldExpectedBehavior_WhenCondition`.
* **Commits:** `[agent] <scope>: <subject>`
* **Logs:** UTC timestamps in filename; Markdown body in English.

## 6. Extending This Charter

* Add new agent → create §2 entry and update §3 ownership.
* All updates must be submitted as dedicated PR.
* Mark proposals with `NEEDS_HUMAN_DECISION`.

## 7. Task Submission Protocol

All new development or documentation tasks must follow the structure defined in `TASK_TEMPLATE.md`.

### Rules:
- Only agents or the human maintainer may add entries to `TASKLOG.md`.
- Each task must include:
  - A linked milestone
  - An assigned agent (matching §2 and §3)
  - A future `docs/progress/` log file expectation
- Optional: reference to a `Specs/*.md` file
- If a task is ambiguous or unassignable → mark with `NEEDS_HUMAN_DECISION`.

### Format:
See `TASK_TEMPLATE.md` for the canonical input format.

> Task entries that do not follow the template must be flagged by the Maintainer agent.

## 8. Project Map

All agents must consult `docs/SUMMARY.md` before exploring the repository. The file provides a high-level directory overview, dependency graph and keyboard map so agents can quickly locate the relevant components.
The Maintainer agent ensures this summary is kept in sync after structural changes.


> “Modularity means putting every idea in the right place—no more, no less.”
