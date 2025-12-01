using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterQualityApi.Data;
using WaterQualityApi.Models;

namespace WaterQualityApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StationsController : ControllerBase
    {
        private readonly WaterQualityContext _context;

        public StationsController(WaterQualityContext context)
        {
            _context = context;
        }

        // GET: api/v1/stations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Station>>> GetStations()
        {
            return await _context.Stations.ToListAsync();
        }

        // GET: api/v1/stations/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Station>> GetStation(int id)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station == null) return NotFound();
            return station;
        }

        // POST: api/v1/stations
        [HttpPost]
        public async Task<ActionResult<Station>> PostStation(Station station)
        {
            _context.Stations.Add(station);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetStation), new { id = station.Id, version = "1.0" }, station);
        }

        // PUT: api/v1/stations/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutStation(int id, Station station)
        {
            if (id != station.Id) return BadRequest();

            _context.Entry(station).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/v1/stations/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStation(int id)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station == null) return NotFound();

            _context.Stations.Remove(station);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
