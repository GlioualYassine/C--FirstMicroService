using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microservice_2_Inventory.Clients
{
    public class CatalogClient
    {
        HttpClient httpclient;
        public CatalogClient(HttpClient http)
        {
            httpclient = http;
        }
        public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemsAsync()
        {
            var items = await httpclient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/items");
            return items; 
        }
    }
}
