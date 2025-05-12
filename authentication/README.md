# Overview of ASP.NET Core authentication

- Authentication is **handled by** the authentication service, `IAuthenticationService`, which is used by **authentication middleware**.
- Authentication service **uses registered authentication handlers** to complete authentication-related actions.
- \*Registered **authentication handlers** + their configuration **options** = **schemes** (aka mechanisms)
- **Authentication schemes example:**
    - `AddAuthentication` parameter sets the **default scheme**.
    - If **multiple schemes** are used, authorization attributes can [specify the authentication scheme(s)](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/limitingidentitybyscheme?view=aspnetcore-8.0) they depend on to authenticate the user.
    - Sometimes, `AddAuthentication` is **called internally** by other extension methods, such as when using **ASP.NET Core Identity**. 

    ```csharp
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
            options => builder.Configuration.Bind("JwtSettings", options))
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            options => builder.Configuration.Bind("CookieSettings", options));
    ```
- Call `UseAuthentication` to register the middleware that uses the previously registered authentication schemes.

## Authentication concepts

- Authentication is **responsible for providing the `ClaimsPrinciple`** for authorization to make permission decisions against.
- **Schemes** are useful for referring to the **authentication, challenge, and forbid behaviors** of the associated handler.
- It's possible to:
    - Specify **different default schemes** for authenticate, challenge, and forbid actions.
    - **Combine multiple schemes** using [policy schemes](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/policyschemes?view=aspnetcore-8.0).

### Authenticate

- Constructs the **user's identity** (`ClaimsPrinciple`) based on **request context**.
- Example:

    Authentication scheme | Behavior
    -|-
    Cookie | Constructs the user's identity from cookies.
    JWT Bearer | **Deserializes and validates** a JWT bearer token to construct the user's identity.

### Challenge

- Called by **Authorization** when an **UNAUTHENTICATED user** requests an endpoint that requires authentication.
- Example:

    Authentication scheme | Behavior
    -|-
    Cookie | Redirects the user to a login page.
    JWT Bearer | Returns a **401** response status code with a `www-authenticate: bearer` header.

### Forbid

- Called by **Authorization** when an **AUTHENTICATED user** attempts to access a resource they're not permitted to access.
- Example:

    Authentication scheme | Behavior
    -|-
    Cookie | Redirects the user to a page indicating access was forbidden.
    JWT Bearer | Returns a **403** response status code.
    Custom | Redirects to a page where the user can request access.

## Authentication providers per tenant (multi-tenant)

- **DOESN'T HAVE** a built-in solution for multi-tenant authentication.
- Recommended OSS:
    - [Orchard Core](https://www.orchardcore.net/) - Modular, multi-tenant app framework, CMS
    - [ABP Framework](https://www.orchardcore.net/) - Supports various architectural patterns.
    - [Finbuckle.MultiTenant](https://www.finbuckle.com/multitenant) - Lightweight, data isolation, unique app behavior for each tenant.

# Choose an identity management solution

- A **_user_** might be a **PERSON** or **ANOTHER APP**.

## Basic identity management with ASP.NET Core Identity

- **ASP.NET Core Identity** 
    - A built-in authentication/identity provider (IdP).
    - Uses **cookie-based** authentication (default).
    - Provides the option to acquire a **token** during authentication.
        - Can be used by mobile and desktop clients.
    - **Cookies are preferred** over tokens for **security** and **simplicity**.
- \***Features:**
    - APIs
    - UI
    - Backend database configuration
    - Storing user credentials
    - Granting or revoking permissions
    - [External logins](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/social/?view=aspnetcore-8.0)
    - [Two-factor authentication (2FA)](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/2fa?view=aspnetcore-8.0)
    - [Password management](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-8.0) - Password recovery
    - Account lockout and reactivation
    - Authenticator apps - TOTP
- \*In most cases, this provider alone is sufficient.

## Determine if an OIDC server is needed

- Web is **stateless**, so web apps require a way to **_remember_ past actions (state)**.
- Common solution - _cookies_
    - Easy to use, but **only works within a single website or web domain**.
- Another solution - _tokens_ (e.g. in JWT format)
    - **Main advantage** - **Not tied to** a specific app or domain.
    - **Main disadvantage** - **Require a service** (e.g. OIDC server) to issue and provide validation for tokens.
    - Usually _signed_ with **asymmetric cryptography**.
- **Common REASON** an OIDC server is required:
    - To expose web-based APIs that are **consumed by OTHER APPS**.
        - \***Client UIs** (e.g. SPA, mobile, desktop) are considered to be part of the **same app**.
    - If you **only have client UIs**, you can just use **ASP.NET Core Identity**.
        - **Not suitable** for managing access from **third-party apps**. 
- **Another REASON** an OIDC server is required:
    - \***Single sign on (SSO)** - To share sign-ins with **OTHER APPS**.
        - OIDC server is **preferred** to provide a **secure and scalable** solution.
- OIDC server deployment solution:
    - Self-host (OpenIddict, IdentityServer)
    - Cloud services (Microsoft Entra ID)

## Disconnected scenarios

- No internet connection.
- ASP.NET Core Identity works well in this case.
- If OIDC server is required, choose a self-host solution.

## ASP.NET Core Identity vs OIDC server

![identity-management-decision-flow](https://learn.microsoft.com/en-us/aspnet/core/security/_static/identity-management-decision-flow.png?view=aspnetcore-8.0)

# Introduction to Identity on ASP.NET Core

- **NOT related** to the **Microsoft identity platform** (alternative identity solution).
- Add UI login functionality to **ASP.NET Core web apps**. Examples:
    - /Identity/Account/Login
    - /Identity/Account/Logout
    - /Identity/Account/Manage
    - /Identity/Account/RegisterConfirmation?email=\<email>
- Solutions to secure web APIs and SPAs:
    - [Microsoft Entra ID](https://learn.microsoft.com/en-us/azure/api-management/api-management-howto-protect-backend-with-aad)
    - [Azure Active Directory B2C (Azure AD B2C)](https://learn.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-custom-rest-api-netfw)
    - [Duende Identity Server](https://docs.duendesoftware.com/) (OIDC server)
