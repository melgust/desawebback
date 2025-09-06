using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessageApi.Data;
using HelloApi.Models;
using HelloApi.Dtos;

namespace HelloApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ItemController(AppDbContext db) => _db = db;

        // GET: /api/Item
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemReadDto>>> GetAll()
        {
            var items = await _db.Set<Item>()
                .AsNoTracking()
                .OrderBy(i => i.Id)
                .ToListAsync();

            return items.Select(ToReadDto).ToList();
        }

        // GET: /api/Item/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ItemReadDto>> GetById(int id)
        {
            var item = await _db.Set<Item>().AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            if (item is null) return NotFound();
            return ToReadDto(item);
        }

        // POST: /api/Item
        [HttpPost]
        public async Task<ActionResult<ItemReadDto>> Create(ItemCreateDto dto)
        {
            var now = DateTime.UtcNow;

            var item = new Item
            {
                Name      = dto.Name,
                Price     = dto.Price,
                CreatedBy = dto.CreatedBy ?? 1,  // por defecto 1
                CreatedAt = now,
                UpdatedBy = dto.CreatedBy ?? 1,
                UpdatedAt = null
            };

            _db.Set<Item>().Add(item);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, ToReadDto(item));
        }

        // PUT: /api/Item/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ItemReadDto>> Update(int id, ItemUpdateDto dto)
        {
            var item = await _db.Set<Item>().FirstOrDefaultAsync(i => i.Id == id);
            if (item is null) return NotFound();

            item.Name      = dto.Name;
            item.Price     = dto.Price;
            item.UpdatedBy = dto.UpdatedBy ?? item.UpdatedBy;
            item.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return ToReadDto(item);
        }

        // DELETE: /api/Item/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Set<Item>().FirstOrDefaultAsync(i => i.Id == id);
            if (item is null) return NotFound();

            _db.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        private static ItemReadDto ToReadDto(Item i) => new ItemReadDto
        {
            Id        = i.Id,
            Name      = i.Name,
            Price     = i.Price,
            CreatedAt = i.CreatedAt,
            UpdatedAt = i.UpdatedAt
        };
    }
}
