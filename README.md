# Library Management System

## Overview
This project is a Library Management System built using ASP.NET Core MVC. It allows users to manage authors and books, including adding, editing, deleting, and viewing details. The application utilizes cookie-based authentication for secure access.

## Features
- **Author Management**: Add, edit, delete, and view authors.
- **Book Management**: Add, edit, delete, and view books.
- **Authentication**: Secure login and access control.

## Technologies Used
- ASP.NET Core MVC
- Entity Framework Core (for data access)
- SQL Server (for database)
- HTML, CSS (Bootstrap for styling)

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Visual Studio](https://visualstudio.microsoft.com/) (or any IDE of your choice)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/library-management-system.git
   cd library-management-system
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Configure the database**
   - Update the connection string in `appsettings.json` to point to your SQL Server instance:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=your_server;Database=LibraryDB;User Id=your_username;Password=your_password;"
     }
     ```

4. **Run database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

### Testing

- Open your web browser and navigate to `http://localhost:5000` to access the application.
- Use the following default credentials to log in:
  - Username: `admin`
  - Password: `password`

### Usage
- **Authors**: Navigate to the "Authors" section to add new authors or manage existing ones.
- **Books**: Go to the "Books" section to manage book records.
- **Authentication**: Users will be redirected to the login page if they attempt to access protected resources without authentication.

## Contributing
Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch (`git checkout -b feature/YourFeature`).
3. Make your changes and commit them (`git commit -m 'Add some feature'`).
4. Push to the branch (`git push origin feature/YourFeature`).
5. Open a pull request.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Bootstrap Documentation](https://getbootstrap.com/docs/5.0/getting-started/introduction/)
