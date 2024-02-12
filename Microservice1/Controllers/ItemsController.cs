using Common.Repositories;
using Microservice1.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice1.Controllers
{
    [ApiController]
    [Route("Items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _itemsRepository;
        public ItemsController(IRepository<Item> itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await _itemsRepository.GetAllAsync()).Select(item=>item.AsDto());
            return items; 
        }
        [HttpGet("{id}")] // Get items/123 123->ID
        public async Task <ActionResult<ItemDto>> GetItemByIdAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);
            if (item != null)
            {
                return item.AsDto(); ;
            }
            return NotFound(); 
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createdItemDto)
        {
            var item = new Item
            {
                Name = createdItemDto.Name,
                Description = createdItemDto.Descripiton,
                Price = createdItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetItemByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto item)
        {
            //find the item to change 
            var existingItem = await _itemsRepository.GetAsync(id);
            if (existingItem == null)
                return NotFound();
            existingItem.Name = item.Name; 
            existingItem.Price = item.Price;
            existingItem.Description = item.Descripiton;

            await _itemsRepository.UpdateAsync(existingItem);    
  
            return NoContent();

        }

        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteAsync(Guid id)
        { 
            var existingItem = await _itemsRepository.GetAsync(id);
            if (existingItem == null)
                return NotFound();
            await _itemsRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
