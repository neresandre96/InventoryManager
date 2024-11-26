using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryManager.API.Data.App.Interfaces;
using InventoryManager.API.Data.App.Models.Types;

namespace InventoryManager.API.Data.App.Models.Entities
{
    public class Ingredient : IServiceable
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name must be at most 50 characters long.")]
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The MinWeight must be a positive value.")]
        public double MinWeight { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The ActualWeight must be a positive value.")]
        public double ActualWeight { get; set; }

        [NotMapped]
        public bool IsBelowMin => ActualWeight < MinWeight;

        [Required(ErrorMessage = "The MeasureUnit field is required.")]
        public UnitOfMeasure MeasureUnit { get; set; }

        [Required(ErrorMessage = "TenantId is required.")]
        public int TenantId { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
