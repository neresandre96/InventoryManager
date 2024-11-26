using InventoryManager.API.Data.Auth.Interfaces;
using InventoryManager.API.Data.Auth.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace InventoryManager.API.Data.Auth.Context
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) :
        IdentityDbContext<User, Role, int>(options),
        IUserDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
