using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using InventoryManager.API.Data.App.Interfaces;
using InventoryManager.API.Data.App.Models.Entities;

namespace InventoryManager.API.Data.App.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) :
         DbContext(options), IAppDbContext
    {

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Movement> Movements { get; set; }


        public int TenantId { get; private set; }

        public void SetTenantId(int tenantId)
        {
            TenantId = tenantId;
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
            {
                entry.Property(nameof(TenantId)).CurrentValue = TenantId;
            }

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
            {
                entry.Property(nameof(TenantId)).IsModified = false;
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.HasQueryFilter(i => EF.Property<int>(i, "TenantId") == TenantId);

                entity.HasIndex(i => i.Name).IsUnique();
            });


            modelBuilder.Entity<Movement>(entity =>
            {
                entity.HasQueryFilter(m => EF.Property<int>(m, "TenantId") == TenantId);


                entity.HasOne(m => m.Ingredient)
                    .WithMany()
                    .HasForeignKey(m => m.IngredientId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
