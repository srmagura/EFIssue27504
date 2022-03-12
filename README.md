# Repro for Entity Framework Core [Issue #27504](https://github.com/dotnet/efcore/issues/27504)

This repository demonstrates a failure to add a migration after renaming an entity class and its `DbSet` property on the `DbContext`.

## Reproduction Steps

1. Open the solution file in Visual Studio 2022 on Windows.
2. In Package Manager Console: `Add-Migration Initial`
3. In `AppDataContext.cs` at the line `public DbSet<DbImport> Imports => Set<DbImport>();`, rename `DbImport` to `DbPageImport` and rename `Imports` to `PageImports`. Use the Visual Studio rename command — this is Ctrl+R, Ctrl+R on my keymap though I'm not sure if that's the default.
4. In Package Manager Console: `Add-Migration Test`

**Expected behavior:** `Add-Migration Test` succeeds.

**Actual behavior:** `Add-Migration Test` fails with

```text
System.ArgumentException: An item with the same key has already been added. Key: System.Object Item [System.String]
   at System.Collections.Generic.Dictionary`2.TryInsert(TKey key, TValue value, InsertionBehavior behavior)
   at Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer.GetSortedProperties(IEntityType entityType, ITable table)
   at Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer.GetSortedColumns(ITable table)
   at Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer.Add(ITable target, DiffContext diffContext)+MoveNext()
   at Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer.DiffCollection[T](IEnumerable`1 sources, IEnumerable`1 targets, DiffContext diffContext, Func`4 diff, Func`3 add, Func`3 remove, Func`4[] predicates)+MoveNext()
   at System.Linq.Enumerable.ConcatIterator`1.MoveNext()
   at Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer.Sort(IEnumerable`1 operations, DiffContext diffContext)
   at Microsoft.EntityFrameworkCore.Migrations.Internal.MigrationsModelDiffer.GetDifferences(IRelationalModel source, IRelationalModel target)
   at Microsoft.EntityFrameworkCore.Migrations.Design.MigrationsScaffolder.ScaffoldMigration(String migrationName, String rootNamespace, String subNamespace, String language)
   at Microsoft.EntityFrameworkCore.Design.Internal.MigrationsOperations.AddMigration(String name, String outputDir, String contextType, String namespace)
   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.AddMigrationImpl(String name, String outputDir, String contextType, String namespace)
   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.AddMigration.<>c__DisplayClass0_0.<.ctor>b__0()
   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.OperationBase.<>c__DisplayClass3_0`1.<Execute>b__0()
   at Microsoft.EntityFrameworkCore.Design.OperationExecutor.OperationBase.Execute(Action action)
```

## Notes

- If the `DbProject` entity is removed, the error goes away.
- If the `DbFileRef` owned type is removed, the error goes away.
