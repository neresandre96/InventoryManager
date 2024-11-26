using InventoryManager.API.Data.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.API.Data.Auth.Interfaces
{
    public interface IUserDbContext
    {
        DbSet<User> Users { get; set; }
    }
}
