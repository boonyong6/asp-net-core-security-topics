using AspNetCoreIdentitySandbox.CustomStorageProviders.Data;
using AspNetCoreIdentitySandbox.CustomStorageProviders.Entities;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentitySandbox.CustomStorageProviders.Repositories;

public class CustomUserStore : IUserStore<ApplicationUser>
{
    private readonly IUsersTable _usersTable;

    public CustomUserStore(IUsersTable usersTable)
    {
        _usersTable = usersTable;
    }

    public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null) throw new ArgumentNullException(nameof(user));

        return _usersTable.CreateAsync(user);
    }

    public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
