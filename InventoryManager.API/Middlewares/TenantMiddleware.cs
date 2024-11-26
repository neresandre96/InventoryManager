using InventoryManager.API.Data.App.Interfaces;
using InventoryManager.API.Providers.Interfaces;

namespace InventoryManager.API.Middlewares
{
    public class TenantMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, IAppDbContext dbContext, ITenantProvider tenantProvider)
        {
            var tenantId = tenantProvider.GetTenantId();

            if (tenantId != default)
            {
                dbContext.SetTenantId(tenantId);
            }

            await _next(context);
        }
    }
}
