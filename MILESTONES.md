# Milestones

## M1: Project Setup
- Establish repository structure with docs, src, and tests directories.
- Add MIT license and initial documentation.

## M2: Cross-Platform Core
- Create a .NET Standard library for business logic. [Done]
- Ensure unit test coverage for core services. [Done]

## M3: WPF UI Prototype
- Integrate MVVM pattern with sample screens.
- Keyboard navigation and theme support.
- Invoice list navigation and creation flow.
- Long list paging with PgUp/PgDn support. [Done]
- Invoice editor redesign with summary tables. [Done]
- Toolbar with Save/Print/Export actions and restored main menu. [Done]
- UX failsafe navigation and visible database location. [Done]
- Menu system baseline with keyboard access. [Menu System: Implemented baseline]

## M3.1: Persistence, Filters and Settings
- Switch to SQLite repositories with schema migration. [Done]
- Add filter dialogs for date, supplier, product group and product. [Done]
- CRUD maintenance for suppliers and products. [Done]
- Settings window for theme and preferences. [Done]
- Specification drafted with agent task list. [Done]
- Acceptance no longer requires screenshot inclusion.
- Global error handling with log file and in-memory fallback. [Done]
 - Enhanced SQLite error handling for locked or corrupt database. [Done]
   - Message when the DB file is locked. [Done]
   - Automatic recovery on corruption with `.bak` backup. [Done]
   - Missing file recreated on startup. [Done]

M3.1 complete – proceeding with M4 features and preparing M5 release candidate.

## M4: Plugin Framework
- Implement plugin loader and example plugin. [Done]
- Define extension points and loading mechanism. [Done]
- Document plugin API. [Done]

## M5: Release Preparation
- Build setup.sh automation script. [Done]
- Publish single-file executable. [Done]
- Generate Windows installer via Inno Setup. [Done]
- Global font scale setting. [Done]
