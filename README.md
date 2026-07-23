# Emergency Support

Web application for managing emergency requests, responders, assignments, notifications, feedback, and activity logs.

Includes an ASP.NET Core MVC site for day-to-day operations and a separate Web API that exposes CRUD endpoints for selected entities.

---

## Features

**MVC Application:**
- User registration and cookie-based login
- Emergency requests: create, list, edit, delete (own requests only)
- Request status updates (`Assigned`, `Completed`) for operators and responders
- Priority level management (`Low`, `Medium`, `High`) for operators
- Responder management: register, list, edit, delete
- Request assignments and notifications
- Feedback submission for completed requests
- Activity logs (operators only)

**Web API:**
- `Users` — GET all, GET by id, POST (create/update), DELETE
- `Responders` — GET all, GET by id, POST (create/update), DELETE
- `ReportsLogs` — GET all, GET by id, POST (create/update), DELETE

---

## Tech Stack

| Layer | Technology |
|---|---|
| MVC | ASP.NET Core 8 MVC, Razor Views |
| API | ASP.NET Core 8 Web API, Swagger (Swashbuckle 6.6.2) |
| ORM | Entity Framework Core 8.0.24 |
| Database | SQL Server |
| Frontend | Bootstrap (superhero theme), Bootstrap Icons, jQuery Validation |

---

## Architecture

EmergencySupport.Web.sln
├── EmergencySupport.Web/ # MVC application
├── EmergencySupport.API/ # Web API
├── EmergencySupport.Data/ # DbContext
├── EmergencySupport.Entities/ # Entity classes
├── EmergencySupport.Repos/ # Repository classes
├── EmergencySupport.Models/ # View models
└── EmergencySupport.Shared/ # Shared utilities


**Flow:**
- MVC app calls repositories → repositories use `EsupportDbContext`
- API calls `EsupportDbContext` directly (no repository layer)
- Both share the same SQL Server database

---

## Database

**Name:** `EsupportDb`

| Table | Purpose |
|---|---|
| Users | Accounts with role-based access (admin, user, operator, responder) |
| EmergencyRequests | Requests with type, description, location, priority, status |
| Responders | Responder profiles linked to users |
| Assignments | Linking table between requests and responders |
| Notifications | User notifications |
| Feedback | Ratings and comments for completed requests |
| ReportsLogs | Activity logs |

**Default connection string:**

Data Source=localhost\SQLEXPRESS01;Initial Catalog=EsupportDb;TrustServerCertificate=True;Integrated Security=True;


Update `ConnectionStrings:EsupportDb` in both `appsettings.json` files if your SQL Server instance differs.

---

## Authentication & Authorization

**Cookie-based authentication** with role-based access control:

| Role | Permissions |
|---|---|
| `admin` | Full access to responders and assignments |
| `emergency operator` | Manage priority levels, update request status, view activity logs |
| `responder` | Update request status, manage responder assignments |
| `user` | Create/edit/delete own requests, submit feedback |

**Session:** 30-minute cookie expiration

**MVC routes:**
- Login: `/Auth/Login`
- Sign up: `/Auth/Signup`
- Access denied: `/Auth/Denied`

**API:** No authentication currently configured

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (default: `localhost\SQLEXPRESS01`)
- Database `EsupportDb` with tables matching entity models

---

## Setup

**1. Restore dependencies:**
```bash
dotnet restore EmergencySupport.Web.sln
```

**2. Update connection strings** in both `appsettings.json` files if needed:
```json
"ConnectionStrings": {
  "EsupportDb": "Data Source=YOUR_SERVER;Initial Catalog=EsupportDb;..."
}
```

**3. Ensure database exists** with tables matching entity definitions

---

## Running the Application

**MVC (Terminal 1):**
```bash
dotnet run --project EmergencySupport.Web
```
- HTTPS: https://localhost:7019
- HTTP: http://localhost:5001

**API (Terminal 2):**
```bash
dotnet run --project EmergencySupport.API
```
- HTTPS: https://localhost:7137
- HTTP: http://localhost:5138
- Swagger UI: https://localhost:7137/swagger (Development only)

**Next steps:**
1. Register at `/Auth/Signup`
2. Log in at `/Auth/Login`
3. Create and manage emergency requests

---

## API Reference

**Base:** `api/[controller]`

### Users
| Method | Route | Description |
|---|---|---|
| GET | `/api/Users/getUsers` | All users |
| GET | `/api/Users/byID/{id}` | User by ID |
| POST | `/api/Users` | Create/update user |
| DELETE | `/api/Users/{id}` | Delete user |

### Responders
| Method | Route | Description |
|---|---|---|
| GET | `/api/Responders/getResponders` | All responders |
| GET | `/api/Responders/byID/{id}` | Responder by ID |
| POST | `/api/Responders` | Create/update responder |
| DELETE | `/api/Responders/{id}` | Delete responder |

### ReportsLogs
| Method | Route | Description |
|---|---|---|
| GET | `/api/ReportsLogs/getReports` | All logs |
| GET | `/api/ReportsLogs/byID/{id}` | Log by ID |
| POST | `/api/ReportsLogs` | Create/update log |
| DELETE | `/api/ReportsLogs/{id}` | Delete log |

**Example:**
```http
GET https://localhost:7137/api/Users/getUsers
```

---

## Notes

- No EF Core migrations included; database schema must already exist
- API has no authentication configured (recommend adding before production use)
- Bootstrap theme and styling via CDN

---

## License

Educational purposes only.

---

## Author

Md. Fahim Karim  
GitHub: [@fahimkarim01](https://github.com/fahimkarim01)
