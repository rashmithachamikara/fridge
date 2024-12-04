using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Data;
using backend.Dtos;
using backend.Models;
using backend.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController: ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ItemController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return Ok(await _context.Item.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Item>> Create([FromBody] ItemDto itemDto)
        {
            var item = itemDto.ToModel();
            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Item>> Update(int id, [FromBody] ItemDto itemDto)
        {
            var itemModel = await _context.Item.FindAsync(id);

            if (itemModel == null)
            {
                return NotFound();
            }

            itemModel.Name = itemDto.Name;
            itemModel.ExpiryDate = itemDto.ExpiryDate;

            _context.SaveChanges();

            return Ok(itemModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> Delete(int id)
        {
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }
        
    }
}