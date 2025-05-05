# 🕒 Timesheet System

A simple time-tracking system built with **ASP.NET Core** for employee login and logout management.

## 📦 Project Structure

- **TimesheetSystem.Domain** – Contains core domain entities like `ApplicationUser` and `Timesheet`.
- **TimesheetSystem.Application** – Contains service interfaces and implementations.
- **TimesheetSystem.Infrastructure** – Handles database context and persistence logic using Entity Framework Core.
- **TimesheetSystem.Api** – Exposes RESTful APIs with JWT authentication.
- **TimesheetSystem.Ui** – ASP.NET MVC frontend that communicates with the API via `HttpClient`.
- **TimesheetSystem.Tests** – Unit testing project for business logic.

## 🚀 Getting Started

### 1. Setup Database

Make sure SQL Server is installed and configured. Then run the following to apply migrations:

```bash
dotnet ef database update --project TimesheetSystem.Infrastructure

dotnet run --project TimesheetSystem.Api

dotnet run --project TimesheetSystem.Ui

Three Screen 
1- Register
2- Login
3- Time Sheet Table 

