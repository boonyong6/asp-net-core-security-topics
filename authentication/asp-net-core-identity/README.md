# Introduction to Identity on ASP.NET Core

- **NOT related** to the **Microsoft identity platform** (alternative identity solution).
- Add UI login functionality to **ASP.NET Core web apps** (you can scaffold all the pages to find out all the routes). Examples:
  - /Identity/Account/Login
  - /Identity/Account/Logout
  - /Identity/Account/Manage
  - /Identity/Account/RegisterConfirmation?email=\<email>
- Solutions to secure web APIs and SPAs:
  - [Microsoft Entra ID](https://learn.microsoft.com/en-us/azure/api-management/api-management-howto-protect-backend-with-aad)
  - [Azure Active Directory B2C (Azure AD B2C)](https://learn.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-custom-rest-api-netfw)
  - [Duende Identity Server](https://docs.duendesoftware.com/) (OIDC server)

## Scaffold Register, Login, Logout, and RegisterConfirmation

- Install package to scaffold identity pages:

  ```bash
  dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version 8.0.7
  ```

- Command to scaffold identity pages:

  ```bash
  dotnet aspnet-codegenerator identity -dc ApplicationDbContext --files "Account.Register;Account.Login;Account.Logout;Account.RegisterConfirmation"
  ```

### Examine Register - `Register.cshtml.cs`

- `IUserStore<TUser>` - Provides API to set the user name.
- `IUserEmailStore<TUser>` - Provides API to set the user email.
- `UserManager<TUser>` - Provides APIs to manage user aggregate and its persistence store.
  - `CreateAsync(TUser)` - Create a new user.
  - `GenerateEmailConfirmationTokenAsync(TUser)` - Generate a token for email confirmation.

### Disable default account verification - `RegisterConfirmation.cshtml.cs`

- Default `Account.RegisterConfirmation` is used **only** for testing.
- Automatic account verification should be disabled in production.
  ```csharp
  public class RegisterConfirmationModel : PageModel
  {
      public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
      {
          // ...
          DisplayConfirmAccountLink = true;
          // ...
      }
  }
  ```

### Log in, log out - `Login.cshtml.cs`, `Logout.cshtml.cs`

- `SignInManager<TUser>` - Provides APIs related to sign in.
  - `PasswordSignInAsync()`
  - `SignOutAsync()` - Clears the **user's claims** stored in a cookie.

## Test Identity

- Annotate classes or methods with `[Authorize]`.

## Identity Components

- **Primary package** - `Microsoft.AspNetCore.Identity` (included by `Microsoft.AspNetCore.Identity.EntityFrameworkCore`)

## `AddDefaultIdentity` and `AddIdentity`

- Calling `AddDefaultIdentity` is similar to calling `AddIdentity`, `AddDefaultUI` and `AddDefaultTokenProviders`.

## Prevent publish of static Identity assets

- **Use case** - When you are not using the default UI.

  ```xml
  <PropertyGroup>
    <ResolveStaticWebAssetsInputsDependsOn>RemoveIdentityAssets</ResolveStaticWebAssetsInputsDependsOn>
  </PropertyGroup>

  <Target Name="RemoveIdentityAssets">
    <ItemGroup>
      <StaticWebAsset Remove="@(StaticWebAsset)" Condition="%(SourceId) == 'Microsoft.AspNetCore.Identity.UI'" />
    </ItemGroup>
  </Target>
  ```

# How to secure Web APIs

- Authentication schemes:

  | Scheme       | Application |
  | ------------ | ----------- |
  | Cookie-based | Browser     |
  | Token-based  | Mobile      |

## Install NuGet packages

- Commands to install the **main** _Identity_ package and _EF Core_ package:

  ```bash
  dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.16
  dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.16
  dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.16
  ```

## Test the API

### Test login (support two authentication schemes)

- **Cookie-based** authentication
  - `/login?useCookies=true`
- **Token-based** authentication (custom token)
  - **Proprietary** to the ASP.NET Core Identity (not standard JWT).
  - Primarily for simple scenarios (not OAuth2).
  - Returns [AccessTokenResponse](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.bearertoken.accesstokenresponse).
  - Call `/refresh` endpoint when the access token is about to expire.

## Log out

- Cookie-based authentication
  - Requires to define a custom logout endpoint and call `await signInManager.SignOutAsync()`.
- Token-based authentication
  - Just delete the token from the client storage.

## [The `MapIdentityApi<TUser>` endpoints](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-api-authorization?view=aspnetcore-9.0#the-mapidentityapituser-endpoints)

- Included endpoints:

  - `POST /register`
  - `POST /login`
  - `POST /refresh` - For use only with token-based authentication.
  - `GET /confirmEmail`

    - Upon `POST /register`, an email will be sent containing a request link to this endpoint.
    - To set up email confirmation:

      ```csharp
      // Program.cs

      builder.Services.Configure<IdentityOptions>(options =>
      {
          options.SignIn.RequireConfirmedEmail = true;
      });
      // [Optional] Customize emails sent.
      builder.Services.AddTransient<IEmailSender, EmailSender>();
      ```

    - More info: [Account confirmation and password recovery in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-9.0)

  - `POST /resendConfirmationEmail`
  - `POST /forgotPassword`
    - Generates an **email** that contains a password **reset code**.
  - `POST /resetPassword`
    - Uses the reset code generated by `/forgotPassword` to set a new password.
  - `POST /manage/2fa`

    1. Send `{}` to initialize the shared key for the authenticator app.
    2. Set up the authenticator app using the generated shared key.
       - More info: [Enable QR code generation for TOTP authenticator apps in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity-enable-qrcodes?view=aspnetcore-9.0)
    3. Enable 2FA by sending:

       - Returns **recovery codes** (used to log in when the authenticator app isn't available).

       ```json
       {
         "enable": true,
         "twoFactorCode": "<TOTP>"
       }
       ```

    4. \[Optional] Reset the **recovery codes**:

       ```json
       {
         "resetRecoveryCodes": true
       }
       ```

    5. \[Optional] Reset the **shared key**:

       - **Disables the 2FA** (needs to be re-enabled).

       ```json
       {
         "resetSharedKey": true
       }
       ```

    6. Forget the machine to force passing of 2FA code for next login:

       - For cookie-based authentication, you don't have to pass the 2FA code every time you log in, as Identity has a mechanism to handle this via cookies.

       ```json
       {
         "forgetMachine": true
       }
       ```

  - `GET /manage/info`
  - `POST /manage/info`

    - Updates the email address and password of the logged-in user.

    ```json
    {
      "newEmail": "string",
      "newPassword": "string",
      "oldPassword": "string"
    }
    ```

# Add, download, and delete custom user data

- Command to scaffold a custom user class and identity pages:

  ```csharp
  dotnet aspnet-codegenerator identity -u ApplicationUser -fi "Account.Register;Account.Manage.Index"
  ```
