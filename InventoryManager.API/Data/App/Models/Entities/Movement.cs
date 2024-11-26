using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryManager.API.Data.App.Interfaces;

namespace InventoryManager.API.Data.App.Models.Entities
{
    public class Movement : IServiceable
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "The IngredientId field is required.")]
        [ForeignKey("IngredientId")]
        public int IngredientId { get; set; }

        [NotMapped]
        public Ingredient Ingredient { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name must be at most 50 characters long.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "IsEntry is required.")]
        public bool IsEntry { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be a positive value greater than zero.")]
        public double Quantity { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public double Price { get; set; } = 0;

        [Required(ErrorMessage = "The TenantId field is required.")]
        public int TenantId { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "The CreatedAt field is required.")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "The UpdatedAt field is required.")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
