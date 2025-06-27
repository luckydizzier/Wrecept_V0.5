# Entity Framework Core Migration

## Context
Current repositories use Dapper with manual SQL statements for SQLite persistence. This approach duplicates CRUD logic across multiple classes and complicates future schema changes.

## Objectives
- Introduce Entity Framework Core as the unified ORM layer.
- Replace existing Dapper repositories with EF Core implementations.
- Keep the SQLite database and existing schema to avoid data migration.

## Constraints
- Domain models remain unchanged.
- Offline-first behaviour and single-file deployment must be preserved.
- Performance should remain acceptable for small local databases.

## Tasks
1. **Architect** – Add EF Core packages and create `WreceptDbContext` with `DbSet` properties for all entities.
2. **CodeGen-CSharp** – Implement EF-based repository classes conforming to existing interfaces.
3. **CodeGen-CSharp** – Register the new DbContext and repositories in DI, replacing Dapper implementations.
4. **TestWriter** – Update repository tests to work with EF Core.
5. **DocWriter** – Document the switch to EF Core in `architecture.md` and `dev_setup.md`.
6. **Architect** – Remove Dapper-specific code once EF repositories are stable.
