using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentitySandbox.Mvc.Entities;

public class ApplicationUser : IdentityUser
{
    // Custom navigation property.
    // `virtual` is used to support lazy loading.
    public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; } = new List<IdentityUserClaim<string>>();
}
