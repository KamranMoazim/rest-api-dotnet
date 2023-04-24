using System;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItemsController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await _itemRepository.GetItemsAsync()).Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await _itemRepository.GetItemAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await _itemRepository.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await _itemRepository.GetItemAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }

            Item item = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await _itemRepository.UpdateItemAsync(item);

            return NoContent();
        }


        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = _itemRepository.GetItemAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }

            await _itemRepository.DeleteItemAsync(id);

            return NoContent();
        }

    }
}