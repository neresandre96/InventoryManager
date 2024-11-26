namespace InventoryManager.API.Data.App.Models.Dtos
{
    public class MovementDto
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public bool IsEntry { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
    }
}
