# SAST Findings — Custom Semgrep Rule

## Overview

This document records the findings from running the custom Semgrep rule
against the VulnerableSecurityAPI project as part of a DevSecOps lab exercise.

---

## Finding #1

| Field     | Detail                                                       |
|-----------|--------------------------------------------------------------|
| Rule ID   | `csharp-sql-injection-fromsqlraw-variable-interpolation`     |
| File      | `Services/UserService.cs`                                    |
| Lines     | 96–99                                                        |
| Severity  | **ERROR (Blocking)**                                         |
| Finding   | Potential SQL Injection                                      |

---

## Vulnerable Code

```csharp
// Line 96 — interpolated string built from user input
var sql = $"SELECT * FROM Users WHERE Username = '{username}'";

// Line 98–99 — interpolated variable passed directly to raw SQL executor
return await _context.Users
    .FromSqlRaw(sql)
```

---

## Root Cause

The `username` parameter comes directly from the HTTP query string:

```
GET /api/users/search?username=admin
```

It is inserted into an interpolated SQL string with no sanitization:

```csharp
var sql = $"SELECT * FROM Users WHERE Username = '{username}'";
```

That string is then passed to `FromSqlRaw()`, which executes it verbatim
against the SQLite database. The application therefore constructs SQL using
untrusted user-controlled input.

### Attack Example

Normal input:

```
?username=admin
```

Results in:

```sql
SELECT * FROM Users WHERE Username = 'admin'
```

Malicious input:

```
?username=' OR '1'='1
```

Results in:

```sql
SELECT * FROM Users WHERE Username = '' OR '1'='1'
```

This returns **all users** in the database, bypassing the intended filter.
The EF Core log confirmed this executed live during testing.

---

## OWASP Reference

- **Category**: A03:2021 — Injection
- **CWE**: CWE-89 — Improper Neutralization of Special Elements used in an SQL Command

---

## Semgrep Rule That Detected This

```yaml
- id: csharp-sql-injection-fromsqlraw-variable-interpolation
  languages:
    - csharp
  message: "Potential SQL Injection: A variable built from an interpolated string
    is being passed to FromSqlRaw. Use parameterized queries instead."
  severity: ERROR
  patterns:
    - pattern: |
        var $SQL = $"...";
        ...
        $CONTEXT.FromSqlRaw($SQL)
```

The `...` between the two lines means Semgrep will match this pattern
regardless of how many lines of code appear between the variable assignment
and the `FromSqlRaw()` call.

---

## Recommended Remediation

Replace the interpolated string with a **parameterized query**.

EF Core's `FromSqlRaw` supports SQL parameters natively:

```csharp
// SAFE — parameterized query, user input never touches SQL structure
return await _context.Users
    .FromSqlRaw("SELECT * FROM Users WHERE Username = {0}", username)
    .Select(u => new UserDto { ... })
    .ToListAsync();
```

Alternatively, use LINQ which is safe by default:

```csharp
// SAFE — LINQ translates to a parameterized query internally
return await _context.Users
    .Where(u => u.Username == username)
    .Select(u => new UserDto { ... })
    .ToListAsync();
```

---

## Remediation Verification

| Stage               | Semgrep Result  | Detail                                              |
|---------------------|-----------------|-----------------------------------------------------|
| Vulnerable code     | 🚨 1 finding    | `var sql = $"..."; .FromSqlRaw(sql)` — interpolated |
| After remediation   | ✅ 0 findings   | `.FromSqlRaw("... {0}", username)` — parameterized  |

**The rule correctly distinguishes between the vulnerable and safe implementations.**

The fix used:

```csharp
// SAFE — EF Core treats {0} as a parameter, not raw SQL
.FromSqlRaw("SELECT * FROM Users WHERE Username = {0}", username)
```

EF Core internally maps `{0}` to a database parameter (`@p0`),
so user input is **never concatenated into the SQL string**.

---

## Scan Command Used

```bash
semgrep --config .\security\semgrep\custom-rules.yml
```
