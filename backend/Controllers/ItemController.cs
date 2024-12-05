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
    public class ItemController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly string _databaseProvider;

        // Constructor: Accept DbContext as a dependency
        public ItemController(DbContext context, IConfiguration configuration)
        {
            _context = context;
            _databaseProvider = configuration["DatabaseProvider"];
            if (string.IsNullOrEmpty(_databaseProvider))
            {
                throw new InvalidOperationException("DatabaseProvider is not configured in the settings.");
            }

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return Ok(await _context.Set<Item>().ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Set<Item>().FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Item>> Create([FromBody] ItemDto itemDto)
        {
            if (itemDto.Name == null || itemDto.ExpiryDate == null)
            {
                return BadRequest();
            }

            if (await _context.Set<Item>().AnyAsync(i => i.Name == itemDto.Name))
            {
                return Conflict("Item with the same name already exists!");
            }

            var item = itemDto.ToModel();
            _context.Set<Item>().Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Item>> Update(int id, [FromBody] ItemDto itemDto)
        {
            var itemModel = await _context.Set<Item>().FindAsync(id);

            if (itemModel == null)
            {
                return NotFound();
            }

            itemModel.Name = itemDto.Name;
            itemModel.ExpiryDate = itemDto.ExpiryDate;

            await _context.SaveChangesAsync();

            return Ok(itemModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Item>> Delete(int id)
        {
            var item = await _context.Set<Item>().FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Set<Item>().Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }
    }
}
