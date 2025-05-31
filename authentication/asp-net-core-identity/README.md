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
  dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
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
