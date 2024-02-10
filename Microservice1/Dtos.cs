using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Microservice1
{
    public record ItemDto(Guid Id , string Name, string Description , decimal Price , DateTimeOffset CreatedDate);
    

    public record CreateItemDto([Required]string Name , string Descripiton ,[Range(0,1000)] Decimal Price );

    public record UpdateItemDto([Required]string Name, string Descripiton, [Range(0, 1000)] Decimal Price);

}
