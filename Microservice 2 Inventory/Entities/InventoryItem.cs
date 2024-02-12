using Common.Entities;
using System;

namespace Microservice_2_Inventory.Entities
{
    public class InventoryItem : IEntity
    {
        public  Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CatalogItemId { get; set; } 
        public int Quantity { get; set; }
        public DateTimeOffset AcquiredDate { get; set; }
    }
}
