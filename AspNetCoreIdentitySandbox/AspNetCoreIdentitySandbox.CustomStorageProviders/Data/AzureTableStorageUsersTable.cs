using AspNetCoreIdentitySandbox.CustomStorageProviders.Entities;
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentitySandbox.CustomStorageProviders.Data;

public class AzureTableStorageUsersTable : IUsersTable
{
    private const string TableName = "users";

    private readonly TableClient _client;

    public AzureTableStorageUsersTable(TableServiceClient serviceClient)
    {
        _client = serviceClient.GetTableClient(TableName);
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        Response response = await _client.AddEntityAsync(user, cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        await _client.DeleteEntityAsync(user, cancellationToken:  cancellationToken);
        return IdentityResult.Success;
    }

    public void Dispose()
    {
        // Nothing to dispose.
    }

    public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        Response<ApplicationUser> response = await _client.GetEntityAsync<ApplicationUser>(userId, userId, cancellationToken: cancellationToken);
        return response.Value;
    }

    public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        AsyncPageable<ApplicationUser> results = _client.QueryAsync<ApplicationUser>(
            user => user.NormalizedUserName == normalizedUserName, cancellationToken: cancellationToken);

        await foreach (ApplicationUser user in results)
        {
            return user;
        }

        return null;
    }
}
