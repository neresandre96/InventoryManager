namespace InventoryManager.API.Data.App.Interfaces
{
    public interface IServiceable
    {
        int ID { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        int TenantId { get; set; }
        string Name { get; set; }
    }
}
