using Microservice1.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice1.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T item);
        Task UpdateAsync(T item);  
        Task DeleteAsync(Guid id);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
    }
}
