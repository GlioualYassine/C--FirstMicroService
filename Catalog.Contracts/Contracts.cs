using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Contracts
{
    public record CatalogItemCreated(Guid itemId , string Name, string Drescription);
    public record CatalogItemUpdated(Guid itemId, string Name, string Drescription);

    public record CatalogItemDeleted(Guid itemId);

}
