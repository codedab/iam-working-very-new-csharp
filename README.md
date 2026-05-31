# Enterprise Identity & Access Management System

A .NET 8 ASP.NET Core Web API implementing user registration, JWT authentication, role-based access control, and protected routes.

## Installation

```bash
dotnet restore
```

## Start

```bash
dotnet run
```

Binds to `http://0.0.0.0:8080` via the `ASPNETCORE_URLS` environment variable.

## Environment Variables

| Variable             | Default                    | Description                        |
|----------------------|----------------------------|------------------------------------|
| `ASPNETCORE_URLS`    | `http://+:8080`            | Binding URL — reads port from here |
| `ASPNETCORE_ENVIRONMENT` | `Production`           | Set to `Test` for test profile     |
| `Jwt__Secret`        | (see appsettings.json)     | HMAC-SHA256 signing key            |

## Endpoints

### `GET /health` — Health Check
Returns HTTP 200.

### `POST /api/users/register` — Register User
```json
{ "username": "alice", "email": "alice@example.com", "password": "Alice@1234" }
```
Returns 201 on success, 400 for missing fields, 409 if email taken.

### `POST /api/auth/login` — Login
```json
{ "email": "alice@example.com", "password": "Alice@1234" }
```
Returns `{ "token": "<jwt>" }` on success, 401 for invalid credentials.
Never reveals which field was wrong.

### `GET /api/users/me` — Current User (protected)
Requires `Authorization: Bearer <token>`. Returns 401 without it.

### `GET /api/roles` — List Roles
Returns `{ "roles": ["admin", "user", "viewer", "manager"] }`.

## Security Notes
- Passwords hashed with BCrypt (cost factor 11)
- JWT signed with HMAC-SHA256
- Failed logins return generic "Invalid credentials" — no email/password enumeration
- All protected routes use `[Authorize]` — client-supplied identity claims are never trusted
