using Microsoft.AspNetCore.Mvc;
using dotnet_api.Repositories;
using dotnet_api.Entities;
using dotnet_api.DTOs;
using dotnet_api;

namespace dotnet_api.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDTO>> GetItemsAsync()
        {
            return (await this.repository.GetItemsAsync()).Select(item => item.AsDTO());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItemAsync(Guid id)
        {
            var item = await this.repository.GetItemAsync(id);
            
            if (item is null) {
                return NotFound();
            }
            return Ok(item.AsDTO());
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> CreateItemAsync(CreateItemDTO itemDTO)
        {
            Item item = new() 
            {   
                id = Guid.NewGuid(),
                name = itemDTO.name,
                price = itemDTO.price,
                created = DateTimeOffset.UtcNow,
            };

            await this.repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new { id = item.id }, item.AsDTO());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDTO itemDTO)
        {
            var existingItem = await this.repository.GetItemAsync(id);
            
            if (existingItem is null)
            {
                return NotFound();
            }

            Item updatedItem = existingItem with 
            {
                name = itemDTO.name,
                price = itemDTO.price
            };

            await this.repository.UpdatedItemAsync(updatedItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await this.repository.GetItemAsync(id);
            
            if (existingItem is null)
            {
                return NotFound();
            }

            await this.repository.DeleteItemAsync(id);

            return NoContent();
        }
    }
}