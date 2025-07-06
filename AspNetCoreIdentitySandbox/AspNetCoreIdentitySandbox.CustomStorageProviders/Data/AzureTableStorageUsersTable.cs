using AspNetCoreIdentitySandbox.CustomStorageProviders.Entities;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentitySandbox.CustomStorageProviders.Data;

public class AzureTableStorageUsersTable : IUsersTable
{
    public Task<IdentityResult> CreateAsync(ApplicationUser user)
    {
        // TODO: Save user to Azure Table Storage.
        throw new NotImplementedException();
    }
}
