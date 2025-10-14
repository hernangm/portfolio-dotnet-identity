# Portfolio.Dotnet.Identity

[![Portfolio.Dotnet.Identity CI](https://github.com/hernangm/portfolio-dotnet-identity/actions/workflows/azure-webapps-dotnet-core.yml/badge.svg)](https://github.com/hernangm/portfolio-dotnet-identity/actions/workflows/azure-webapps-dotnet-core.yml)


## Functional Description

This project is an Identity Server built with .NET 9, designed to handle authentication and authorization for various applications. It supports:

*   **User Management:** Registration, login, password management, and profile editing.
*   **Authentication:** Local user accounts and external identity providers (Azure AD, Google).
*   **Authorization:** Role-based access control for APIs and applications.
*   **Customization:** Configurable password policies, email integration, and client/resource management.
*   **Security:** Implements best practices for data protection and secure communication.

## Technical Description

*   **Technology Stack:**
    *   .NET 9: Latest .NET runtime for performance and features.
    *   IdentityServer4: Industry-standard framework for OpenID Connect and OAuth 2.0.
    *   Entity Framework Core: ORM for data access.
    *   ASP.NET Core Razor Pages: For user interface elements.
    *   Swagger/OpenAPI: For API documentation and exploration.
*   **Architecture:**
    *   Modular design with clear separation of concerns.
    *   Configuration-driven to support different environments.
    *   Uses dependency injection for testability and maintainability.
    *   Implements custom profile service and redirect URI validator for enhanced flexibility.
*   **Database:**
    *   Uses SQL Server as the data store.
    *   EF Core migrations for database schema management.
*   **Email Integration:**
    *   Supports sending emails for user registration and password reset.
*   **External Identity Providers:**
    *   Supports Azure Active Directory and Google authentication.
*   **Development Practices:**
    *   Uses Swagger for API documentation.
    *   Configured for development and production environments.
    *   Includes CORS configuration for cross-origin requests.