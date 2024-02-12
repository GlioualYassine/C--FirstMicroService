using System;

namespace Microservice_2_Inventory
{
    public record GrantItemDtos(Guid userId , Guid CatalogItemId, int Quantity);
    public record InventoryItemDto(Guid CatalogItemId,string Name,string Description,int Quantity , DateTimeOffset AcquiredDate);
    public record CatalogItemDto(Guid Id, string Name, string Description);

}
