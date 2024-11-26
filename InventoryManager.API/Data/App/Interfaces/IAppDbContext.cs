using InventoryManager.API.Data.App.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.API.Data.App.Interfaces
{
    public interface IAppDbContext
    {

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbSet<Ingredient> Ingredients { get; set; }
        DbSet<Movement> Movements { get; set; }

        int SaveChanges();
        void SetTenantId(int tenantId);
    }
}