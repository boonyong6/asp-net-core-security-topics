# ASP.NET Core Security Topics

## Overview

- Topics:
    - Authentication
    - Authorization
    - Data protection
    - HTTPS enforcement
    - Safe storage of app secrets in development
    - [XSRF/CSRF prevention](https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-9.0)
    - Cross Origin Resource Sharing (CORS)
    - [Cross-Site Scripting (XSS) attacks](https://learn.microsoft.com/en-us/aspnet/core/security/cross-site-scripting?view=aspnetcore-9.0)
    - [SQL injection attacks](https://learn.microsoft.com/en-us/ef/core/querying/raw-sql)
    - [Open redirect attacks](https://learn.microsoft.com/en-us/aspnet/core/security/preventing-open-redirects?view=aspnetcore-9.0)

## ASP.NET Core security features

- Built-in identity providers.
- Third-party identity services.
- Approaches to store app secrets.

## Secure authentication flows

- [Managed identities](https://learn.microsoft.com/en-us/entra/identity/managed-identities-azure-resources/overview) - The **most secure** authentication for **Azure services**.
- ⛔ Avoid _Resource Owner Password Credentials Grant_.
- ⛔ Against using environment variables to store a production connection string.
- Use **Secret Manager tool** to store secrets in development.

## Enterprise web app patterns

- [Reference](https://learn.microsoft.com/en-us/azure/architecture/web-apps/guides/enterprise-app-patterns/overview)