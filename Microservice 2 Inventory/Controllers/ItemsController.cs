using Common.Repositories;
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

        public ItemsController(IRepository<InventoryItem> repository)
        {
            this._ItemRepository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAync(Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest();
            var items = (await _ItemRepository.GetAllAsync(item => item.UserId == userId)).Select(item=>item.AsDto());
            return Ok(items); 
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemDtos grantItemDtos)
        {
            var InventoryItem = await _ItemRepository.GetAsync(
                item => item.UserId == grantItemDtos.userId && item.CatalogItemId == grantItemDtos.CatalogueItemId);
            if(InventoryItem == null)
            {
                InventoryItem = new InventoryItem
                {
                    CatalogItemId = grantItemDtos.CatalogueItemId,
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
