# Emergency Support System

A full-stack emergency response management platform built with **ASP.NET Core 8**. The system coordinates emergency incidents from initial report through responder dispatch, assignment tracking, notifications, user feedback, and operational audit logging.

---

## Overview

Emergency Support System is designed for scenarios where citizens or staff report emergencies, operators triage and prioritize incidents, and field responders are assigned and tracked until resolution. The solution follows a **layered architecture** with a shared data layer, repository-based business access, an MVC web application for day-to-day operations, and a REST API for programmatic access.

---

## Features

- **User authentication** — Registration, login, and logout with cookie-based authentication and role claims
- **Emergency requests** — Create and manage incidents with type, description, GPS coordinates, priority, and status
- **Responder management** — Maintain responder profiles with service type, availability, and current location
- **Assignments** — Link emergency requests to responders with lifecycle tracking (assigned, arrival, completion)
- **Notifications** — User-specific alerts with read/unread state
- **Feedback** — Post-incident ratings and comments linked to requests
- **Reports & audit logs** — Activity logging for operational review
- **Role-based access control** — Distinct permissions for admins, users, emergency operators, and responders

---

## Tech Stack

| Category | Technology |
|----------|------------|
| Runtime | .NET 8 |
| Web UI | ASP.NET Core MVC |
| API | ASP.NET Core Web API |
| ORM | Entity Framework Core 8 |
| Database | Microsoft SQL Server |
| Frontend | Bootstrap, jQuery Validation |

---

## Solution Architecture
