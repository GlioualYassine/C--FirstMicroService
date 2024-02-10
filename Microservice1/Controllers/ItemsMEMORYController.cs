/*using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservice1.Controllers
{
   // [ApiController]
   // [Route("Items")]
    public class ItemsMEMORYController : ControllerBase
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(),"Potion","Restore a small amount of HP",5,System.DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(),"Antidote","Cures poison",7,System.DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(),"Bronze sword","Deals a small amount of damage",20,System.DateTimeOffset.UtcNow),
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return items;
        }
        [HttpGet("{id}")] // Get items/123 123->ID
        public ActionResult<ItemDto> GetItemById(Guid id) {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            if(item != null) {
                return item;
            }    
            return NotFound();
        }

        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto item)
        {
            var itemcreated = new ItemDto(Guid.NewGuid(), item.Name, item.Descripiton, item.Price, DateTimeOffset.UtcNow);
            items.Add(itemcreated);
            return CreatedAtAction(nameof(GetItemById), new { id = itemcreated.Id, }, itemcreated);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto item)
        {
            var existingItem = items.Where(item => item.Id == id).SingleOrDefault();
            if (existingItem == null)
            {
                return NotFound();
            }
                if (existingItem != null)
            {
                var updatedItem = existingItem with
                {
                    Name = item.Name,
                    Description = item.Descripiton,
                    Price = item.Price
                };

                var index = items.FindIndex(existingItem => existingItem.Id == id);
                items[index] = updatedItem;
            }
            return NoContent();

        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var existingItem = items.Where(items => items.Id == id).SingleOrDefault();
            if (existingItem == null)
            {
                return NotFound();
            }
            items.Remove(existingItem);
            return NoContent(); 
        }

    }
}
*/