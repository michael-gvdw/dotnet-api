using System.ComponentModel.DataAnnotations;

namespace dotnet_api.DTOs
{
    public record UpdateItemDTO
    {
        [Required]
        public string name { get; init;}

        [Required]
        [Range(1, 1000)]
        public decimal price { get; init; }
    }
}