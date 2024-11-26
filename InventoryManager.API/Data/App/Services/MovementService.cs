using InventoryManager.API.Data.App.Interfaces;
using InventoryManager.API.Data.App.Models.Entities;
using InventoryManager.API.Data.App.Models.Types;

namespace InventoryManager.API.Data.App.Services
{
    public class MovementService(IAppDbContext context) :
        EntityService<Movement, IAppDbContext>(context), IMovementService
    {
        private readonly IAppDbContext AppDbContext = context;

        public override object CrudOperation(Movement movement, CrudActions actionType)
        {
            var ingredient = AppDbContext.Ingredients.SingleOrDefault(i => i.ID == movement.IngredientId)
                ?? throw new InvalidOperationException($"Ingredient not found.");

            var entityObj = base.CrudOperation(movement, actionType);

            UpdateInventory(movement);

            return entityObj;
        }

        public virtual void IncreaseInventoryQuantity(Movement movement)
        {
            var ingredient = AppDbContext.Ingredients.SingleOrDefault(i => i.ID == movement.IngredientId) 
                ?? throw new InvalidOperationException($"Ingredient not found.");

            ingredient.ActualWeight += movement.Quantity;

        }


        public virtual void DecreaseInventoryQuantity(Movement movement)
        {
            var ingredient = AppDbContext.Ingredients.SingleOrDefault(i => i.ID == movement.IngredientId)
                ?? throw new InvalidOperationException($"Ingredient not found.");

            Console.WriteLine($"Decreasing inventory quantity for ingredient: {ingredient.Name}");

            ingredient.ActualWeight -= movement.Quantity;

            if (ingredient.ActualWeight < 0)
            {
                ingredient.ActualWeight = 0;
            }
        }

        public void UpdateInventory(Movement movement)
        {
            if (movement.IsEntry)
            {
                IncreaseInventoryQuantity(movement);
            }
            else
            {
                DecreaseInventoryQuantity(movement);
            }
            
            AppDbContext.SaveChanges();
        }
    }
}
