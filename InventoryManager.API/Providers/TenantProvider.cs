using InventoryManager.API.Providers.Interfaces;

namespace InventoryManager.API.Providers
{
    public class TenantProvider(IHttpContextAccessor httpContextAccessor) : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public int GetTenantId()
        {
            var context = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("No current context.");

            var tenantIdClaim = context.User.FindFirst("TenantId");

            if (tenantIdClaim != null)
            {
                if (int.TryParse(tenantIdClaim.Value, out var tenantId)) return tenantId;
                else throw new InvalidOperationException("TenantId is not an integer.");
            }
            else return 0;
            //throw new InvalidOperationException("TenantId not found in the current context.");
        }
    }
}
