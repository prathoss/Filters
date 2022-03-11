using Filters.DataDescriptor;
using Filters.Demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Filters.Demo.Data.Models;
using Filters.Extensions;

namespace Filters.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        private readonly BikeContext _context;

        public BikeController(BikeContext context)
        {
            _context = context;
        }

        // GET: api/Bike
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bike>>> GetBikes()
        {
            return await _context.Bikes.ToListAsync();
        }

        // GET: api/Bike/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bike>> GetBike(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);

            if (bike == null)
            {
                return NotFound();
            }

            return bike;
        }

        // PUT: api/Bike/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBike(int id, Bike bike)
        {
            if (id != bike.Id)
            {
                return BadRequest();
            }

            _context.Entry(bike).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bike
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bike>> PostBike(Bike bike)
        {
            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBike", new { id = bike.Id }, bike);
        }

        // DELETE: api/Bike/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBike(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null)
            {
                return NotFound();
            }

            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("describe")]
        [ProducesResponseType(typeof(Dictionary<string,IEnumerable<object>>), 200)]
        public async Task<IActionResult> Describe([FromQuery(Name = "col")] IEnumerable<string> columns)
        {
            var bikeDescriptor = new DataDescriptor<Bike>()
                .SetPropValuesAsync(b => b.Manufacturer, async () => await _context.Bikes.Select(b => b.Manufacturer).Distinct().ToArrayAsync());
            var response = new Dictionary<string, IEnumerable<object>>();
            foreach (var column in columns)
            {
                response.Add(column, await bikeDescriptor.Describe(column));
            }
            return Ok(response);
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(IEnumerable<Bike>), 200)]
        public async Task<IActionResult> Filter([FromBody] IDictionary<string, FilterModel> filter)
        {
            var list = await _context.Bikes.Filter(filter).ToListAsync();
            return Ok(list);
        }

        private bool BikeExists(int id)
        {
            return _context.Bikes.Any(e => e.Id == id);
        }
    }
}
