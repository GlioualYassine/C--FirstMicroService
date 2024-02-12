using Common.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Repositories;
using System.Linq.Expressions;
namespace Common.Repositories
{
    public class MongoRepository<T>: IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> dbCollection;
        private readonly FilterDefinitionBuilder<T> filterBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            dbCollection = database.GetCollection<T>(collectionName);

        }
        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }
        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).ToListAsync();
        }

        public async Task<T>GetAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity=>entity.Id,id);
            return await dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await dbCollection.Find(filter).FirstOrDefaultAsync();

        }

        public async Task CreateAsync(T item)
        {
            if(item == null) throw new ArgumentNullException(nameof(item));
            await dbCollection.InsertOneAsync(item);
        }
        public async Task UpdateAsync(T updatedItem)
        {
            if (updatedItem == null) throw new ArgumentNullException(nameof(updatedItem));
            FilterDefinition<T> filter = filterBuilder.Eq(existingItem => existingItem.Id,updatedItem.Id);
            await dbCollection.ReplaceOneAsync(filter, updatedItem);
        }
        public async Task DeleteAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(item => item.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }

        
    }
}
