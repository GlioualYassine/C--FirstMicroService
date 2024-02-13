using Common.Repositories;
using Microservice_2_Inventory.Clients;
using Microservice_2_Inventory.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_2_Inventory.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _inventoryItemRepository;
        private readonly IRepository<CatalogItem> _catalogItemRepository;

        public ItemsController(IRepository<InventoryItem> inventoryItemRepository, IRepository<CatalogItem> catalogItemRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _catalogItemRepository = catalogItemRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAync(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest();
            
            var inventoryItemEntities = await _inventoryItemRepository.GetAllAsync(item=>item.UserId == userId);
            var itemIds = inventoryItemEntities.Select(item => item.CatalogItemId);
            var catalogitems = await _catalogItemRepository.GetAllAsync(item=>itemIds.Contains(item.Id));
            var inventoryItemDtos = inventoryItemEntities.Select(inventoryItem =>
            {
                //join operation btw catalogItem Entity and InventoryItem Entity
                var catalogItem = catalogitems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                //here give me the item who has the same id as the inventoryItem in order to get informations about this item
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
                //then return it as a DTO 
            });

            return Ok(inventoryItemDtos); 
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemDtos grantItemDtos)
        {
            var InventoryItem = await _inventoryItemRepository.GetAsync(
                item => item.UserId == grantItemDtos.userId && item.CatalogItemId == grantItemDtos.CatalogItemId);
            if(InventoryItem == null)
            {
                InventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemDtos.CatalogItemId,
                    UserId = grantItemDtos.userId,
                    Quantity = grantItemDtos.Quantity, 
                    AcquiredDate = DateTimeOffset.UtcNow
                };
                await _inventoryItemRepository.CreateAsync(InventoryItem);
            }
            else
            {
                InventoryItem.Quantity += grantItemDtos.Quantity;
                await _inventoryItemRepository.UpdateAsync(InventoryItem);
            }
            return Ok();
        }
    }
}
