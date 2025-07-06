namespace AspNetCoreIdentitySandbox.CustomStorageProviders.Entities;

// TODO: Do I need to inherit from `IdentityUser<TKey, TUserClaim, TUserRole, TUserLogin, TUserToken>` or `IIdentity`?
// Example: https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/security/authentication/identity-custom-storage-providers/sample/CustomIdentityProviderSample/CustomProvider/ApplicationUser.cs
public class ApplicationUser
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
}
