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
        public IEnumerable<ItemDTO> GetItems()
        {
            return this.repository.GetItems().Select(item => item.AsDTO());
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDTO> GetItem(Guid id)
        {
            var item = this.repository.GetItem(id);
            
            if (item is null) {
                return NotFound();
            }
            return Ok(item.AsDTO());
        }

        [HttpPost]
        public ActionResult<ItemDTO> CreateItem(CreateItemDTO itemDTO)
        {
            Item item = new() 
            {   
                id = Guid.NewGuid(),
                name = itemDTO.name,
                price = itemDTO.price,
                created = DateTimeOffset.UtcNow,
            };

            this.repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new { id = item.id }, item.AsDTO());
        }

        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDTO itemDTO)
        {
            var existingItem = this.repository.GetItem(id);
            
            if (existingItem is null)
            {
                return NotFound();
            }

            Item updatedItem = existingItem with 
            {
                name = itemDTO.name,
                price = itemDTO.price
            };

            this.repository.UpdatedItem(updatedItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = this.repository.GetItem(id);
            
            if (existingItem is null)
            {
                return NotFound();
            }

            this.repository.DeleteItem(id);

            return NoContent();
        }
    }
}