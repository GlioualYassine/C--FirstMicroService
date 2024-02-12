using Microservice_2_Inventory.Entities;

namespace Microservice_2_Inventory
{
    public static class Extensions
    {
        public static InventoryItemDto AsDto(this InventoryItem item)
        {
            return new InventoryItemDto(item.CatalogItemId, item.Quantity, item.AcquiredDate);  
        }
    }
}
