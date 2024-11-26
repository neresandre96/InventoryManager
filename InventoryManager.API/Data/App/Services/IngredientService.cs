using InventoryManager.API.Data.App.Interfaces;
using InventoryManager.API.Data.App.Models.Entities;

namespace InventoryManager.API.Data.App.Services
{
    public class IngredientService(IAppDbContext context) :
        EntityService<Ingredient, IAppDbContext>(context),
        IIngredientService
    {
    }
}
