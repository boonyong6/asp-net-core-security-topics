using Azure;
using Azure.Data.Tables;

namespace AspNetCoreIdentitySandbox.CustomStorageProviders.Entities;

// Example: https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/security/authentication/identity-custom-storage-providers/sample/CustomIdentityProviderSample/CustomProvider/ApplicationUser.cs
public class ApplicationUser : ITableEntity
{
    public ApplicationUser() { }

    public ApplicationUser(string id, string userName)
    {
        Id = id;
        UserName = userName;

        PartitionKey = id;
        RowKey = id;
    }

    // ASP.NET Core Identity required properties
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;

    // Other properties
    public string? NormalizedUserName { get; set; }
    //public string UserType { get; set; } = null!;  // E.g. Admin, User, Guest, System, etc.

    // Azure Table storage required properties
    public string PartitionKey { get; set; } = null!;
    public string RowKey { get; set; } = null!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; } = ETag.All;
}
