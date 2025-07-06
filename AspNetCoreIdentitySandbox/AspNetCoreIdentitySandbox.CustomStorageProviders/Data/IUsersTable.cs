using AspNetCoreIdentitySandbox.CustomStorageProviders.Entities;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentitySandbox.CustomStorageProviders.Data;

public interface IUsersTable
{
    Task<IdentityResult> CreateAsync(ApplicationUser user);
}
