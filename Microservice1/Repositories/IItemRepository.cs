using Microservice1.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice1.Repositories
{
    public interface IItemRepository
    {
        Task CreateAsync(Item item);
        Task UpdateAsync(Item item);  
        Task DeleteAsync(Guid id);
        Task<IReadOnlyCollection<Item>> GetAllAsync();
        Task<Item> GetAsync(Guid id);
    }
}
