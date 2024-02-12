using System;

namespace Microservice_2_Inventory
{
    public record GrantItemDtos(Guid userId , Guid CatalogueItemId, int Quantity);
    public record InventoryItemDto(Guid CatalogItemId,int Quantity , DateTimeOffset AcquiredDate);
   
}
