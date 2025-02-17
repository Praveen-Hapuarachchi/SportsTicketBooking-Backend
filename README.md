# 🎟️ SportsTicketBooking-Backend

## 📌 Overview
A comprehensive RESTful API built with **ASP.NET Core 7.0** that handles **sports event ticket bookings**. This individual project demonstrates **modern software architecture principles** with secure authentication, efficient ticket management, and a robust booking system.

## ⭐ Key Features
- 🔐 **Secure Authentication**
  - 🔑 JWT-based token authentication
  - 🏷️ Role-based access control (Admin/User)
  - 🔒 Secure password handling

- 🎟️ **Ticket Management**
  - 📊 Real-time availability tracking
  - 🏆 Automated booking system
  - 🔍 Smart search and filtering
  - 🕒 Booking history

- 🛠️ **Admin Controls**
  - 📌 Complete ticket inventory control
  - 👥 User management dashboard
  - 📈 Booking analytics
  - 🎫 Event management

- 👤 **User Features**
  - ✅ Easy ticket booking
  - 📜 Booking history view
  - ✏️ Profile customization
  - 🔔 Booking notifications

## 🛠️ Technology Stack
- 🚀 **Framework:** ASP.NET Core 7.0
- 🏛️ **ORM:** Entity Framework Core 7.0
- 🗄️ **Database:** MySQL
- 🔑 **Authentication:** JWT (JSON Web Tokens)
- 📜 **Documentation:** Swagger/OpenAPI
- 🧪 **Testing:** xUnit, Moq

## 📋 Prerequisites
- 🖥️ .NET SDK 7.0+
- 🗄️ MySQL Server 8.0+
- 💻 Visual Studio 2022/VS Code
- 🔬 Postman

## 🚀 Getting Started

### ⚙️ Installation

1️⃣ Clone the repository:
```bash
git clone https://github.com/Praveen-Hapuarachchi/SportsTicketBooking-Backend.git
cd SportsTicketBooking-Backend
```

2️⃣ Update the database connection in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SportsTicketDB;User=root;Password=your_password;"
  },
  "JwtSettings": {
    "SecretKey": "your_secure_key_here",
    "Issuer": "sports-ticket-api",
    "Audience": "sports-ticket-clients",
    "ExpirationInMinutes": 60
  }
}
```

3️⃣ Run migrations:
```bash
dotnet ef database update
```

4️⃣ Start the application:
```bash
dotnet run
```

🎯 Access the API at `https://localhost:5001` and Swagger docs at `https://localhost:5001/swagger`

## 📌 API Endpoints

### 🔐 Authentication
| 🛠️ Method | 🔗 Endpoint | 📋 Description | 🔑 Access |
|--------|----------|-------------|---------|
| POST | `/api/auth/register` | Register new user | Public |
| POST | `/api/auth/login` | User login | Public |
| POST | `/api/auth/refresh-token` | Refresh JWT token | Authenticated |

### 🎟️ Tickets
| 🛠️ Method | 🔗 Endpoint | 📋 Description | 🔑 Access |
|--------|----------|-------------|---------|
| GET | `/api/tickets` | List all tickets | Public |
| GET | `/api/tickets/{id}` | Get ticket details | Public |
| POST | `/api/tickets` | Create ticket | Admin |
| PUT | `/api/tickets/{id}` | Update ticket | Admin |
| DELETE | `/api/tickets/{id}` | Remove ticket | Admin |

### 📆 Bookings
| 🛠️ Method | 🔗 Endpoint | 📋 Description | 🔑 Access |
|--------|----------|-------------|---------|
| POST | `/api/bookings` | Create booking | Authenticated |
| GET | `/api/bookings/my` | View my bookings | Authenticated |
| GET | `/api/bookings/{id}` | Get booking details | Authenticated |
| DELETE | `/api/bookings/{id}` | Cancel booking | Authenticated |

## 🔒 Authentication
For protected endpoints, include the JWT token:
```
Authorization: Bearer <your_jwt_token>
```

## 📦 Project Structure
```
SportsTicketBooking-Backend/
├── 📂 Controllers/
│   ├── 📜 AuthController.cs
│   ├── 📜 TicketsController.cs
│   └── 📜 BookingsController.cs
├── 📂 Models/
│   ├── 📜 User.cs
│   ├── 📜 Ticket.cs
│   └── 📜 Booking.cs
├── 📂 Services/
│   ├── 📜 AuthService.cs
│   ├── 📜 TicketService.cs
│   └── 📜 BookingService.cs
├── 📂 Data/
│   └── 📜 ApplicationDbContext.cs
└── 📂 DTOs/
    ├── 📜 AuthDtos.cs
    ├── 📜 TicketDtos.cs
    └── 📜 BookingDtos.cs
```

## ⚠️ Error Handling
Standard HTTP status codes with detailed messages:
- ❌ **400**: Invalid input
- 🔒 **401**: Authentication required
- 🚫 **403**: Insufficient permissions
- 🔍 **404**: Resource not found
- 🔥 **500**: Server error

## 🔮 Future Enhancements
- [ ] 💳 Payment system integration
- [ ] 📲 Real-time notifications
- [ ] 🌍 Multi-language support
- [ ] 📱 Mobile app API
- [ ] 📊 Advanced analytics
- [ ] ⛔ Rate limiting
- [ ] 🚀 Response caching

## 📞 Contact & Support

- 👨‍💻 **Developer:** Praveen Hapuarachchi
- 📧 **Email:** hapup14@gmail.com
- 📞 **Phone:** +94 9255150
- 🔗 **LinkedIn:** [Praveen Hapuarachchi](https://www.linkedin.com/in/praveen-hapuarachchi)
- 🛠️ **Issues:** Please open an issue in the repository

---
💙 Developed with passion by **Praveen Hapuarachchi** 🚀

