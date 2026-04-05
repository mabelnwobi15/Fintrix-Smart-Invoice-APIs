# 📄 Smart Invoice API

A powerful backend API for managing invoices, payments, and customers. Built to support modern web and mobile applications with secure and scalable architecture.

## 🚀 Features
- Create, update, and delete invoices
- Record and track payments
- Manage customers
- Invoice status tracking (Paid, Pending, Overdue)
- RESTful API endpoints
- Database integration (SQL Server / Azure SQL)
- Fast and scalable backend

## 🛠️ Tech Stack
- Backend: ASP.NET Core Web API
- Database: SQL Server
- ORM: Entity Framework Core
- Language: C#
- Authentication: (JWT)

## 📁 Project Structure

- SmartInvoice.API/
- │── Controllers/        # API endpoints
- │── Models/             # Data models
- │── DTOs/               # Data transfer objects
- │── Data/               # Database context
- │── Services/           # Business logic
- │── Migrations/         # EF Core migrations
- │── Program.cs          # Entry point
- │── appsettings.json    # Configuration

## 📡 API Endpoints

Invoices

| Method | Endpoint           | Description        |
| ------ | ------------------ | ------------------ |
| GET    | /api/invoices      | Get all invoices   |
| GET    | /api/invoices/{id} | Get invoice by ID  |
| POST   | /api/invoices      | Create new invoice |
| PUT    | /api/invoices/{id} | Update invoice     |
| DELETE | /api/invoices/{id} | Delete invoice     |

Payments

| Method | Endpoint           | Description       |
| ------ | ------------------ | ----------------- |
| GET    | /api/payments      | Get all payments  |
| GET    | /api/payments/{id} | Get payment by ID |
| POST   | /api/payments      | Create payment    |

Example:

{
  "invoiceId": 1,
  "amount": 500
}

## 📊 Invoice Status Logic
- Paid → Total payments ≥ invoice amount
- Pending → No or partial payments
- Overdue → Due date passed and not fully paid

## 🧪 Testing
You can test the API using:

- Postman
- Swagger UI (if enabled)
- Thunder Client (VS Code)
