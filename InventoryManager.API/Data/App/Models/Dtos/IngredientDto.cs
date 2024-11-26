using InventoryManager.API.Data.App.Models.Types;

namespace InventoryManager.API.Data.App.Models.Dtos
{
    public class IngredientDto
    {
        public string Name { get; set; }
        public double MinWeight { get; set; }
        public double ActualWeight { get; set; }
        public UnitOfMeasure MeasureUnit { get; set; }
    }
}
