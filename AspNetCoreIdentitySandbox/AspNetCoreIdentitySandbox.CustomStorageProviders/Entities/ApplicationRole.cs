namespace AspNetCoreIdentitySandbox.CustomStorageProviders.Entities;

public class ApplicationRole
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
}
