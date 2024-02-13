using Catalog.Contracts;
using Common.Repositories;
using MassTransit;
using Microservice_2_Inventory.Entities;
using System.Threading.Tasks;

namespace Microservice_2_Inventory.Consumers
{
    public class CatalogItemUpdatedConsumers : IConsumer<CatalogItemUpdated>
    {
        private readonly IRepository<CatalogItem> repository;

        public CatalogItemUpdatedConsumers(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }
        //this method is used to consum a message(which is the item created/updated/deleted) from the RabbitMQ( and stored if it doesnt exist in local database CatalogItemInventory)
        // if it already exist dont do anythig return ; 
        public async Task Consume(ConsumeContext<CatalogItemUpdated> context)
        {
            var message = context.Message;
            var item = await repository.GetAsync(message.itemId);
            if (item != null)
            {
                // item is already created into our database so we have only to update the properties
                item.Name = message.Name;
                item.Description = message.Drescription;
                await repository.UpdateAsync(item);
            }
            else
            {
                item = new CatalogItem
                {
                    Id = message.itemId,
                    Name = message.Name,
                    Description = message.Drescription
                };
                await repository.CreateAsync(item);
            }
        }
    }
}
