using Lab1_THKTPM.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Lab1_THKTPM.Data
{
    public interface IIdentitySeed
    {
        Task Seed(UserManager<IdentityUser> userManager,
                  RoleManager<IdentityRole> roleManager,
                  IOptions<ApplicationSettings> options);
    }
}
