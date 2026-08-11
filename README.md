# SAST Security Pipeline for ASP.NET Core API

## Purpose
This application is part of a DevSecOps portfolio project. It will later be intentionally modified to contain security vulnerabilities for SAST testing using tools like Semgrep, SonarQube, and GitHub Actions.

## Current Stage
Stage 1 — Base API

## Technology Stack
- ASP.NET Core (.NET 8)
- C#
- Entity Framework Core
- SQLite
- Swagger
- Git
- GitHub

## Current Features
The base application is currently configured with the following API endpoints:

- **Users**: CRUD operations (`/api/users`)
- **Products**: CRUD operations (`/api/products`)
- **Authentication**: Basic mock login endpoint (`/api/auth/login`)

## Future Security Testing
Later stages of this project will introduce controlled examples of:
- SQL Injection
- Hardcoded Secrets
- Weak Authentication
- Insecure Input Validation
- XSS-related Patterns
- Sensitive Information in Logs

> **WARNING**: This application will be intentionally vulnerable in later stages and must **never** be deployed to a production environment.

## Getting Started

1. **Install dependencies:**
   ```bash
   dotnet restore
   ```

2. **Create the database and apply migrations:**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

3. **Run the API:**
   ```bash
   dotnet run
   ```

4. **Test endpoints:**
   Open the Swagger UI by navigating to the URL displayed in the console (usually `http://localhost:5xxx/swagger`).
