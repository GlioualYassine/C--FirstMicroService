using Microservice1.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservice1.Repositories; 
namespace Microservice1.Repositories
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
        public Task<T>GetAsync(Guid id)
        {
            FilterDefinition<T> filter = filterBuilder.Eq(entity=>entity.Id,id);
            return dbCollection.Find(filter).FirstOrDefaultAsync();
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
