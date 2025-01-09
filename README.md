# MoneyMe Challenge
[![.NET](https://github.com/rjramirez/MoneyMe.Challenge/actions/workflows/dotnet.yml/badge.svg)](https://github.com/rjramirez/MoneyMe.Challenge/actions/workflows/dotnet.yml)

## Welcome to the MoneyMe Challenge

Welcome to the MoneyMe Challenge project! This solution is an Enterprise-grade architecture, Single Page Application (SPA) that uses UoW and Repository pattern that includes various functionalities such as calculating quotes, displaying quote details, and managing blacklists. Follow the instructions below to set up and run the project.

### MoneyMe Challenge

The MoneyMe Challenge is a financial application designed to help users calculate loan quotes, view detailed quote information, and manage blacklists. The application leverages AdminLTE v3.1 for the front-end, providing a modern and responsive user interface.

## Software Details
Design: Domain Driven, Database-first
Type: Microservices
Patterns: Unit of Work(UoW), Repository


## Getting Started

To get started with the MoneyMe Challenge project, follow the setup instructions below.

## Requirements

Before you begin, ensure you have the following installed on your machine:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Setup Instructions

### .NET 8 SDK

1. Download and install the .NET 8 SDK from the [official .NET website](https://dotnet.microsoft.com/download/dotnet/8.0).
2. Verify the installation by running the following command in your terminal or command prompt:

### Visual Studio 2022

1. Download and install Visual Studio 2022 from the [official Visual Studio website](https://visualstudio.microsoft.com/vs/).
2. During the installation, ensure you select the following workloads:
   - ASP.NET and web development
   - .NET desktop development

### SQL Server Setup

1. Download and install SQL Server from the [official SQL Server website](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).
2. Restore the `.bak` file to set up the database:
   - Open SQL Server Management Studio (SSMS).
   - Connect to your SQL Server instance.
   - Right-click on the `Databases` node and select `Restore Database...`.
   - In the `Source` section, select `Device` and click the `...` button.
   - Add the path to the `.bak` file and click `OK`.
   - In the `Destination` section, enter the name of the database.
   - Click `OK` to restore the database.

## Running the Project

1. Open the solution in Visual Studio 2022.
2. Make sure to select projects `WebApp` and `WebApi`.
3. Build the solution by clicking `Build` > `Build Solution` or pressing `Ctrl+Shift+B`.
4. Run the project by clicking `Debug` > `Start Debugging` or pressing `F5`.

## Running Tests

1. Open the solution in Visual Studio 2022.
2. Build the solution by clicking `Build` > `Build Solution` or pressing `Ctrl+Shift+B`.
3. Open the Test Explorer by clicking `Test` > `Test Explorer`.
4. Run all tests by clicking `Run All` in the Test Explorer.

## Project Structure

- **WebApp**: The main project containing the UI and controllers.
- **Common**: Contains common data transfer objects and constants.
- **DataAccess**: Contains the database context, unit of work, and repository pattern implementations.
- **Test**: Contains unit tests for the project.

## Additional Information

- The project uses AdminLTE v3.1 for the front-end with Razor Pages or ASP.NET MVC.
- Ensure that the database connection string in the `WebApi` -> `appsettings.json` file is correctly configured to point to your SQL Server instance.

## Support

If you encounter any issues or have any questions, please feel free to reach out for support at [devrj@outlook.ph](mailto:devrj@outlook.ph).
