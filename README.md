# 📚 BookStore — Full-Stack Online Bookstore Platform

<div align="center">

![.NET Core](https://img.shields.io/badge/.NET_Core-9.0_%7C_8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Angular](https://img.shields.io/badge/Angular-19.2-DD0031?style=for-the-badge&logo=angular&logoColor=white)
![TailwindCSS](https://img.shields.io/badge/Tailwind_CSS-4.1-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)
![DaisyUI](https://img.shields.io/badge/DaisyUI-5.0-5A0EF8?style=for-the-badge&logo=daisyui&logoColor=white)
![Dapper](https://img.shields.io/badge/Dapper-Micro_ORM-E44D26?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL_Server-2019+-CC292B?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![JWT](https://img.shields.io/badge/Auth-JWT_Cookie-black?style=for-the-badge&logo=json-web-tokens)
![ZarinPal](https://img.shields.io/badge/Payment-ZarinPal-FFBE00?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

<p align="center">
  <b>A modern, high-performance, full-stack eCommerce platform for book discovery, purchasing, customer reviews, and store administration.</b>
  <br />
  Built with ASP.NET Core, Dapper, SQL Server, and an Angular 19 SPA featuring Tailwind CSS & DaisyUI with native RTL/Persian support.
</p>

[✨ Features](#-key-features) •
[🏗️ Architecture & Technologies](#️-architecture--technologies) •
[📁 Project Structure](#-project-structure) •
[🚀 Getting Started](#-getting-started) •
[⚙️ Configuration](#️-configuration) •
[📡 API Reference](#-api-reference) •
[🤝 Contributing](#-contributing) •
[📄 License](#-license)

---

</div>

## 📖 About The Project

**BookStore** is a robust, full-stack web application designed for modern digital book retail. The project is engineered with high scalability, maintainability, and clean architecture principles in mind:

- **Backend**: Built with **ASP.NET Core Web API**, following a clean **3-Tier Layered Architecture** (Controllers ➔ Business Logic Layer [BLL] ➔ Data Access Repositories) powered by **Dapper Micro-ORM** for blazing-fast database queries and minimal memory overhead.
- **Frontend**: A sleek single-page application (**SPA**) built on **Angular 19**, styled with **Tailwind CSS 4** and **DaisyUI 5**, offering a responsive, accessible, and RTL-first user experience.
- **Security & Payments**: Secured by **JWT Authentication in HttpOnly cookies**, **SMS.ir OTP validation**, **PBKDF2/SHA-256 password hashing**, and integrated with **ZarinPal Payment Gateway** for seamless transaction processing.

---

## ✨ Key Features

### 🛒 1. Public Storefront
- **Dynamic Homepage**: Highlighting interactive carousels, featured books, bestsellers, newest releases, and category spotlights.
- **Advanced Book Catalog**: Multi-faceted filtering and sorting by category, author, translator, tags, price range, and in-stock status with pagination.
- **Comprehensive Book Details**: High-resolution image galleries, detailed specifications (ISBN, pages, dimensions, publisher, year), author/translator credits, and related tags.
- **Author & Translator Showcases**: Dedicated profile pages featuring biographies, photos, and linked publications.
- **Customer Reviews & Ratings**: User review submissions, star ratings, and moderation workflow.
- **Interactive Shopping Cart**: Client/server cart synchronization, dynamic price calculations, and smooth checkout flow.

### 👤 2. Customer Portal (User Panel)
- **Profile Management**: Update personal info, change avatar, and manage account credentials.
- **Order & Invoice Tracking**: Real-time tracking of order history with statuses (`Pending`, `Paid`, `Failed`, `Canceled`).
- **Wishlist**: Save favorite titles for future purchase or quick reference.
- **Address Book**: Manage multiple shipping addresses with postal codes and contact details.
- **Review History**: View and track user-submitted book reviews and moderation statuses.

### 🛡️ 3. Administrative Control Panel (Admin Dashboard)
- **Book Management (CRUD)**: Create, update, and manage books with multi-category, multi-author, multi-translator, and multi-tag associations.
- **Media & Image Processing**: Automated image resizing, formatting, and optimization using `SixLabors.ImageSharp`.
- **Author & Translator Management**: Manage biographical data, portrait images, and creator profiles.
- **Category & Tag Taxonomy**: Hierarchical category tree and tag classification for book organization.
- **Invoices & Transactions**: Comprehensive logs of customer invoices and verified payment gateway transactions.
- **User Administration**: Role assignment (`Admin`, `User`), status toggling, and user search.
- **Comment Moderation**: Review, approve, reject, and delete incoming book feedback.

### 🔐 4. Authentication & Security
- **JWT Authentication via HttpOnly Cookies**: Protection against XSS and client-side token tampering.
- **SMS OTP Verification**: Integrated with `SMS.ir` for mobile phone verification and passwordless authentication.
- **Secure Password Hashing**: Industry-standard one-way cryptographic hashing (`PBKDF2 / SHA-256`).
- **Role-Based Access Control (RBAC)**: Enforced via `[Authorize(Roles = "...")]` attributes on backend endpoints and Angular Route Guards (`adminGuard`, `userPanelGuard`).

### 💳 5. Payment Gateway
- **ZarinPal Integration**: Official SDK integration supporting seamless switching between **Sandbox** and **Production** environments.
- **Callback Verification**: Instant verification of transaction reference IDs (`RefId`) and automated invoice status updates.

---

## 🏗️ Architecture & Technologies

```
┌────────────────────────────────────────────────────────┐
│               Frontend (Angular 19 SPA)                │
│    Tailwind CSS 4 + DaisyUI 5 + Lucide + Jalaali       │
└───────────────────────────┬────────────────────────────┘
                            │ HTTPS / REST API / JSON
                            ▼
┌────────────────────────────────────────────────────────┐
│             Backend (ASP.NET Core Web API)             │
│  ┌──────────────────────────────────────────────────┐  │
│  │ Controllers Layer (Admin, Public, User, Auth)   │  │
│  └────────────────────────┬─────────────────────────┘  │
│                           ▼                            │
│  ┌──────────────────────────────────────────────────┐  │
│  │ Business Logic Layer (BLL Services)              │  │
│  └────────────────────────┬─────────────────────────┘  │
│                           ▼                            │
│  ┌──────────────────────────────────────────────────┐  │
│  │ Data Access Layer (Repositories + Dapper)        │  │
│  └────────────────────────┬─────────────────────────┘  │
│                           ▼                            │
│  ┌──────────────────────────────────────────────────┐  │
│  │ External Services (JWT, ImageSharp, SMS, Zarin)  │  │
│  └──────────────────────────────────────────────────┘  │
└───────────────────────────┬────────────────────────────┘
                            │ SQL Queries / Dapper
                            ▼
┌────────────────────────────────────────────────────────┐
│            Database (Microsoft SQL Server)             │
└────────────────────────────────────────────────────────┘
```

### 💻 Backend Stack
- **Framework**: ASP.NET Core Web API (.NET 9 / .NET 8)
- **Data Access & ORM**: Dapper Micro-ORM + `Microsoft.Data.SqlClient`
- **Pattern**: 3-Tier Layered Architecture with complete separation between Controllers, BLL, and Repositories
- **DTO Organization**: Segregated by module domain (`Admin`, `Auth`, `Public`, `User`) containing `Mappers`, `QueryObjects`, `Requests`, and `Responses`
- **Image Processing**: `SixLabors.ImageSharp` for on-the-fly image compression and formatting
- **Documentation**: Swagger / OpenAPI 3.0 with JWT Bearer authorization testing
- **Dependency Injection**: Native ASP.NET Core IoC container with scoped services and repositories

### 🎨 Frontend Stack
- **Framework**: Angular 19 (Modular `AppModule` architecture)
- **UI & Styling**: Tailwind CSS 4 + DaisyUI 5
- **Icons**: Lucide Angular (`lucide-angular`)
- **Carousels**: Swiper.js
- **Calendar & Dates**: `jalaali-js` + Custom Angular Persian date pipes
- **State & Data Stream**: RxJS Observables, Angular HTTP Services, and Error/Auth Interceptors

---

## 📁 Project Structure

```plaintext
BookStore/
├── BookStore.sln                 # Main Visual Studio Solution
├── BookStore.sql                 # SQL Server Database Script (Schema & Seed Data)
│
├── BookStoreApi/                 # Backend Project (ASP.NET Core Web API)
│   ├── BusinessLogicLayer/       # Business Logic Layer (BLL)
│   │   ├── Interfaces/           # BLL Contracts (Admin, Public, UserPanel)
│   │   └── LogicLayers/          # BLL Concrete Implementations
│   ├── Controllers/              # Web API REST Controllers
│   │   ├── Admin/                # Admin-only endpoints (Book, Author, Category, Invoice, etc.)
│   │   ├── Public/               # Storefront endpoints (Catalog, Search, Creators, Tags)
│   │   ├── User/                 # User dashboard endpoints (Orders, Profile, Addresses)
│   │   └── AuthController.cs     # Authentication & Registration controller
│   ├── Database/                 # Data Access Layer (DAL)
│   │   ├── DapperUtility.cs      # Reusable Dapper query execution utilities
│   │   ├── Interfaces/           # Repository interfaces
│   │   ├── Models/               # Database entity models
│   │   └── Repositories/         # Repository implementations with raw SQL / Dapper
│   ├── Enums/                    # System enumerations (UserRole, InvoiceStatus, PaymentStatus)
│   ├── RequestHandler/           # Request/Response DTOs and Mappers
│   │   ├── Admin/                # Admin requests, responses, mappers, and query objects
│   │   ├── Auth/                 # Authentication requests/responses
│   │   ├── Public/               # Public requests, responses, and view models
│   │   └── User/                 # User panel requests and responses
│   ├── Services/                 # Infrastructure services (JWT, SMS.ir, ImageService, Hasher)
│   ├── appsettings.json          # Configuration settings and connection strings
│   └── Program.cs                # Entry point, DI container configuration, and middleware pipeline
│
└── BookStoreFront/               # Frontend Project (Angular 19 SPA)
    ├── src/
    │   ├── app/
    │   │   ├── admin/            # Admin dashboard components
    │   │   ├── public/           # Public storefront components (Home, Books, Details, Auth)
    │   │   ├── user/             # User panel components (Profile, Orders, Wishlist, Addresses)
    │   │   ├── guards/           # Angular Route Guards (adminGuard, userPanelGuard)
    │   │   ├── interceptors/     # HTTP interceptors for cookies and error handling
    │   │   ├── models/           # TypeScript interfaces synchronized with backend DTOs
    │   │   ├── pipes/            # Custom pipes (Persian date conversion, Currency formatting)
    │   │   ├── services/         # Angular HTTP services (Admin, Public, User, Auth, Image)
    │   │   ├── app-routing.module.ts # Client-side routing configuration
    │   │   └── app.module.ts     # Root Angular module
    │   └── environments/         # Environment configuration files
    ├── angular.json              # Angular CLI project settings
    └── package.json              # Node.js dependencies and scripts
```

---

## 🚀 Getting Started

### 📋 Prerequisites
Make sure you have the following installed on your development machine:
- **[.NET SDK](https://dotnet.microsoft.com/download)** (Version 8.0 or 9.0+)
- **[Node.js](https://nodejs.org/)** (Version 18.x or 20.x+) & **npm**
- **[Angular CLI](https://angular.dev/tools/cli)** (`npm install -g @angular/cli@19`)
- **[Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)** (2019+ or LocalDB / SQL Server Express)
- **Visual Studio 2022** / **VS Code**

---

### 1. Clone the Repository
```bash
git clone https://github.com/0ehsan-sh0/BookStore-C.git
cd BookStore
```

---

### 2. Database Setup
1. Launch **SQL Server Management Studio (SSMS)** or the SQL Server extension in VS Code.
2. Open the [`BookStore.sql`](file:///c:/Users/Arshian/source/repos/BookStore/BookStore.sql) script located in the root directory.
3. Execute the script to generate the database schema, foreign keys, tables, and initial seed data.

---

### 3. Backend Setup & Run
1. Navigate to the API directory:
   ```bash
   cd BookStoreApi
   ```
2. Verify your database connection string in [`appsettings.json`](file:///c:/Users/Arshian/source/repos/BookStore/BookStoreApi/appsettings.json):
   ```json
   "ConnectionStrings": {
     "Default": "Data Source=.;Initial Catalog=Bookstore;Integrated Security=True;TrustServerCertificate=True"
   }
   ```
3. Restore dependencies and run the project:
   ```bash
   dotnet restore
   dotnet run --launch-profile https
   ```
4. The API will start at `https://localhost:7034`.
5. Access the interactive **Swagger UI** at:
   `https://localhost:7034/swagger`

---

### 4. Frontend Setup & Run
1. Open a new terminal and navigate to the frontend directory:
   ```bash
   cd BookStoreFront
   ```
2. Install npm dependencies:
   ```bash
   npm install
   ```
3. Verify the backend API URL in [`src/environments/environment.ts`](file:///c:/Users/Arshian/source/repos/BookStore/BookStoreFront/src/environments/environment.ts):
   ```typescript
   export const environment = {
     production: false,
     apiUrl: 'https://localhost:7034'
   };
   ```
4. Start the Angular development server:
   ```bash
   ng serve
   ```
5. Navigate to `http://localhost:4200` in your browser.

---

## ⚙️ Configuration

### Backend: `appsettings.json`

| Setting Key | Description | Example / Default |
| :--- | :--- | :--- |
| `ConnectionStrings:Default` | SQL Server database connection string | `Data Source=.;Initial Catalog=Bookstore;...` |
| `JWTConfiguration:Issuer` | JWT Token Issuer URL | `https://localhost:7157/` |
| `JWTConfiguration:Audience` | JWT Token Audience URL | `https://localhost:7157/` |
| `JWTConfiguration:Key` | Symmetric encryption key for JWT signing | `[Your-Secret-JWT-Key]` |
| `JWTConfiguration:TokenValidityMinutes` | JWT expiration duration in minutes | `2880` (48 hours) |
| `Frontend:URL` | Client origin allowed by CORS policy | `http://localhost:4200` |
| `SmsIr:SandboxApiKey` | API key for SMS.ir verification service | `[Your-SMS-IR-API-Key]` |
| `ZarinPal:MerchantId` | Merchant ID for ZarinPal gateway | `00000000-0000-0000-0000-000000000000` |
| `ZarinPal:Sandbox` | Toggle sandbox mode for payment testing | `true` / `false` |

---

## 📡 API Reference

### 🔐 Authentication & Identity
- `POST /api/auth/register` — Register a new customer account
- `POST /api/auth/login` — Authenticate credentials and issue JWT HttpOnly cookie
- `POST /api/auth/send-code` — Request SMS OTP verification code
- `POST /api/auth/logout` — Clear authentication cookie and invalidate session

### 📚 Public Storefront
- `GET /api/public/book` — Get paginated, filtered, and sorted books
- `GET /api/public/book/{id}` — Get full details of a specific book
- `GET /api/public/category` — Retrieve book categories tree
- `GET /api/public/author` — List and search authors
- `GET /api/public/translator` — List and search translators
- `GET /api/public/tag` — Retrieve book taxonomy tags

### 👤 Customer Portal (Requires `User` Role)
- `GET /api/user/panel/profile` — Get authenticated user profile
- `PUT /api/user/panel/profile` — Update user profile details
- `GET /api/user/address` — List stored shipping addresses
- `POST /api/user/address` — Add a new shipping address
- `POST /api/user/purchase/checkout` — Generate invoice and initiate payment gateway redirect
- `GET /api/user/wishlist` — Retrieve customer wishlist items

### 🛡️ Admin Dashboard (Requires `Admin` Role)
- `POST|PUT|DELETE /api/admin/book` — Complete CRUD operations for books
- `POST|PUT|DELETE /api/admin/author` — Manage author records
- `POST|PUT|DELETE /api/admin/translator` — Manage translator records
- `POST|PUT|DELETE /api/admin/category` — Manage category taxonomy
- `GET|PUT /api/admin/invoice` — Inspect and modify customer invoices
- `GET /api/admin/payment` — View bank transaction audit logs
- `GET|PUT /api/admin/comment` — Moderate customer reviews and ratings
- `POST /api/admin/image/upload` — Upload and optimize media assets via ImageSharp

---

## 🤝 Contributing

Contributions make the open-source community an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

Distributed under the **[MIT License](LICENSE.txt)**. See `LICENSE.txt` for more information.

---

<div align="center">
  <sub>Engineered with ❤️ for book lovers and full-stack developers.</sub>
</div>
