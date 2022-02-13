using dotnet_api.Entities;
using dotnet_api.DTOs;

namespace dotnet_api
{
    public static class Extensions
    {
        public static ItemDTO AsDTO(this Item item)
        {
            return new ItemDTO
            {
                id = item.id,
                name = item.name,
                price = item.price,
                created = item.created
            };
        }
    }
}