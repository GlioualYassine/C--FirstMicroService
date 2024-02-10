using Microservice1.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice1.Repositories
{
    public class ItemsRepository:IItemRepository
    {
        private const string collectionName = "Items";
        private readonly IMongoCollection<Item> dbCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public ItemsRepository(IMongoDatabase database)
        {
            dbCollection = database.GetCollection<Item>(collectionName);

        }
        public async Task<IReadOnlyCollection<Item>> GetAllAsync()
        {
            return await dbCollection.Find(filterBuilder.Empty).ToListAsync();
        }
        public Task<Item>GetAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(entity=>entity.Id,id);
            return dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item item)
        {
            if(item == null) throw new ArgumentNullException(nameof(item));
            await dbCollection.InsertOneAsync(item);
        }
        public async Task UpdateAsync(Item updatedItem)
        {
            if (updatedItem == null) throw new ArgumentNullException(nameof(updatedItem));
            FilterDefinition<Item> filter = filterBuilder.Eq(existingItem => existingItem.Id,updatedItem.Id);
            await dbCollection.ReplaceOneAsync(filter, updatedItem);
        }
        public async Task DeleteAsync(Guid id)
        {
            FilterDefinition<Item> filter = filterBuilder.Eq(item => item.Id, id);
            await dbCollection.DeleteOneAsync(filter);
        }
    }
}
