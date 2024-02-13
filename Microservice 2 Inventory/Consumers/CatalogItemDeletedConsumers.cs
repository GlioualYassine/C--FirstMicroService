using Catalog.Contracts;
using Common.Repositories;
using MassTransit;
using Microservice_2_Inventory.Entities;
using System.Threading.Tasks;

namespace Microservice_2_Inventory.Consumers
{
    public class CatalogItemDeletedConsumers : IConsumer<CatalogItemDeleted> // here we declare an event if there is smth published related to CatalogItemDeleted we call Consome method
    {
        private readonly IRepository<CatalogItem> repository;

        public CatalogItemDeletedConsumers(IRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }
        //this method is used to consum a message(which is the item created/updated/deleted) from the RabbitMQ( and stored if it doesnt exist in local database CatalogItemInventory)
        // if it already exist dont do anythig return ; 
        public async Task Consume(ConsumeContext<CatalogItemDeleted> context)
        {
            var message = context.Message;
            var item = await repository.GetAsync(message.itemId);
            if(item == null)
            {
                // item do not exist dont do anything
                return;
            }
            else
            {
                // but if the item aleardy exist , it have to be deleted
                await repository.DeleteAsync(item.Id);   
            }
            
        }
    }
}
