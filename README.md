# GoodLuck

This project is an ASP.NET Core web application for managing special messages.
It now includes simple pages to keep letters and anniversaries in an
in‑memory database so no files are created on disk.

## Getting Started

1. Ensure .NET 8 SDK is installed.
2. Restore packages with `dotnet restore`.
3. Run the project with `dotnet run`.

### Working with migrations

If you need to create database migrations, first install the EF Core CLI
tool:

```bash
dotnet tool install --global dotnet-ef
```

Then run `dotnet ef migrations add <Name>` from the project directory.

Because the database is in memory, no database file will be generated when
running the application.
