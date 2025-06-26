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
- Startup opens main invoice list window. [Done]
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
- Global font scale setting. [Removed]
- Inline creator component for quick product entry. [Done]

## M6: Post Release Cleanup
- Extend InlineCreator for Supplier master data. [Done]
- Implement Hungarian number-to-text conversion for invoice totals. [Done]
- Finalize placeholder row logic and invoice part initialization. [Done]
- Add unit tests for NavigationService dialogs. [Done]
## M7: Entity Lookup Integration
- Lookup dialogs accessible from name fields (Product, Supplier, Product Group, Unit, TaxRate).
- Keyboard navigation with F2 or Ctrl+L, Enter to select, Esc to cancel.
- Documentation and tests for new feature.

## M7.1: Polishing
- CancelEdit restores supplier and payment method
- Async loading wrapped with error handling
- Input lock management via IDisposable scope
- Unique invoice numbering independent of list count
- Fix invoice list crash when filter result is empty
- Disable row addition in Supplier and Product grids
- Graceful handling for settings save and invoice export errors [Done]
- Default/Cancel actions on filter and settings dialogs [Done]
- Cancel placeholder row entry with Esc [Done]
- Close Help and About windows with Esc [Done]
- F1 overlay closes with Esc [Done]

## M7.2: Feedback System
- Unified audio and visual cues for actions and validation
