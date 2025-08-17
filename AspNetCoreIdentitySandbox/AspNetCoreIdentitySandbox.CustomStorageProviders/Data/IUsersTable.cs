using AspNetCoreIdentitySandbox.CustomStorageProviders.Entities;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentitySandbox.CustomStorageProviders.Data;

public interface IUsersTable
{
    Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken);
    Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken);
    Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken);
    Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
    void Dispose();
}
