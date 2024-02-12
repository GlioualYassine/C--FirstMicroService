using Microservice_2_Inventory.Entities;

namespace Microservice_2_Inventory
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item,string Name , string Description)
        {
            return new InventoryItemDto(item.CatalogItemId,Name,Description, item.Quantity, item.AcquiredDate);  
        }
    }
}
