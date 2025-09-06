using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelloApi.Dtos;
using HelloApi.Models;
using MessageApi.Data;

namespace HelloApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<ActionResult<OrderReadDto>> Create(OrderCreateDto dto)
        {
            if (dto.Details == null || dto.Details.Count == 0)
                return BadRequest("La orden debe tener al menos un detalle.");

            var person = await _db.Set<Person>().FirstOrDefaultAsync(p => p.Id == dto.PersonId);
            if (person == null)
                return BadRequest($"No existe Person con Id {dto.PersonId}.");

            var lastNumber = await _db.Set<Order>().MaxAsync(o => (int?)o.Number) ?? 0;
            var nextNumber = lastNumber + 1;

            var now = DateTime.UtcNow;
            var userId = dto.CreatedBy ?? 1;

            var order = new Order
            {
                PersonId = dto.PersonId,
                Number = nextNumber,
                CreatedBy = userId,
                CreatedAt = now,
                UpdatedBy = userId,
                UpdatedAt = null,
                OrderDetails = new List<OrderDetail>()
            };

            foreach (var d in dto.Details)
            {
                if (d.Quantity <= 0) return BadRequest("Quantity debe ser mayor a cero.");

                var item = await _db.Set<Item>().FirstOrDefaultAsync(i => i.Id == d.ItemId);
                if (item == null) return BadRequest($"No existe Item con Id {d.ItemId}.");

                var price = item.Price;
                var total = price * d.Quantity;

                order.OrderDetails.Add(new OrderDetail
                {
                    Order = order,
                    OrderId = order.Id,  // requerido por tu modelo (se actualiza al guardar)
                    ItemId = d.ItemId,
                    Quantity = d.Quantity,
                    Price = price,
                    Total = total,
                    CreatedBy = userId,
                    CreatedAt = now,
                    UpdatedBy = userId,
                    UpdatedAt = null
                });
            }

            _db.Set<Order>().Add(order);
            await _db.SaveChangesAsync();

            var readDto = await _db.Set<Order>()
                .Where(o => o.Id == order.Id)
                .Select(o => new OrderReadDto
                {
                    Id = o.Id,
                    Number = o.Number,
                    PersonId = o.PersonId,
                    PersonName = o.Person.FirstName,
                    CreatedAt = o.CreatedAt,
                    Details = o.OrderDetails.Select(od => new OrderDetailReadDto
                    {
                        Id = od.Id,
                        ItemId = od.ItemId,
                        ItemName = od.Item.Name,
                        Price = od.Price,
                        Quantity = od.Quantity,
                        Total = od.Total
                    }).ToList(),
                    GrandTotal = o.OrderDetails.Sum(od => od.Total)
                })
                .FirstAsync();

            return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderReadDto>> GetById(int id)
        {

            var readDto = await _db.Set<Order>()
                .Where(o => o.Id == id)
                .Select(o => new OrderReadDto
                {
                    Id = o.Id,
                    Number = o.Number,
                    PersonId = o.PersonId,
                    PersonName = o.Person.FirstName, 
                    CreatedAt = o.CreatedAt,
                    Details = o.OrderDetails.Select(od => new OrderDetailReadDto
                    {
                        Id = od.Id,
                        ItemId = od.ItemId,
                        ItemName = od.Item.Name,
                        Price = od.Price,
                        Quantity = od.Quantity,
                        Total = od.Total
                    }).ToList(),
                    GrandTotal = o.OrderDetails.Sum(od => od.Total)
                })
                .FirstOrDefaultAsync();

            if (readDto == null) return NotFound();
            return readDto;
        }
    }
}