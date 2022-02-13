using System;

namespace dotnet_api.Entities
{
    public record Item 
    {
        public Guid id { get; init; }
        public string? name { get; init; }
        public decimal price { get; init; }
        public DateTimeOffset created { get; init; }
    }
}   