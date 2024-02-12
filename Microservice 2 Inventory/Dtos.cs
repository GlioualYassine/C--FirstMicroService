using System;

namespace Microservice_2_Inventory
{
    public record GrantItemDtos(Guid userId , Guid CatalogItemId, int Quantity);
    public record InventoryItemDto(Guid CatalogItemId,int Quantity , DateTimeOffset AcquiredDate);
   
}
