using InventoryManager.API.Data.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace InventoryManager.API.Providers
{
    public class CustomUserClaimsPrincipalFactory(
        UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<IdentityOptions> optionsAccessor) :
        UserClaimsPrincipalFactory<User, Role>(userManager, roleManager, optionsAccessor)
    {
        public async override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);

            ((ClaimsIdentity)principal.Identity).AddClaims([
                new Claim("TenantId", user.Id.ToString())
            ]);

            return principal;
        }
    }

}
