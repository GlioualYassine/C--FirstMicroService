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
        private readonly IRepository<InventoryItem> _ItemRepository;
        private readonly CatalogClient catalogClient; 
        public ItemsController(IRepository<InventoryItem> repository,CatalogClient catalog)
        {
            this._ItemRepository = repository;
            this.catalogClient = catalog;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAync(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest();
            var catalogitems = await catalogClient.GetCatalogItemsAsync();
            var inventoryItemEntities = await _ItemRepository.GetAllAsync(item=>item.UserId == userId);
            var inventoryItemDtos = inventoryItemEntities.Select(inventoryItem =>
            {
                var catalogItem = catalogitems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
            });

            return Ok(inventoryItemDtos); 
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemDtos grantItemDtos)
        {
            var InventoryItem = await _ItemRepository.GetAsync(
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
                await _ItemRepository.CreateAsync(InventoryItem);
            }
            else
            {
                InventoryItem.Quantity += grantItemDtos.Quantity;
                await _ItemRepository.UpdateAsync(InventoryItem);
            }
            return Ok();
        }
    }
}
