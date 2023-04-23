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
        public IEnumerable<ItemDto> GetItems()
        {
            var items = _itemRepository.GetItems().Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id:Guid}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = _itemRepository.GetItem(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            _itemRepository.CreateItem(item);
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());
        }

        [HttpPut("{id:Guid}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = _itemRepository.GetItem(id);
            if (existingItem is null)
            {
                return NotFound();
            }

            Item item = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            _itemRepository.UpdateItem(item);

            return NoContent();
        }


        [HttpDelete("{id:Guid}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = _itemRepository.GetItem(id);
            if (existingItem is null)
            {
                return NotFound();
            }

            _itemRepository.DeleteItem(id);

            return NoContent();
        }

    }
}