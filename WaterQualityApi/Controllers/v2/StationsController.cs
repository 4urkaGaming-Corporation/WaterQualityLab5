using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterQualityApi.Data;

namespace WaterQualityApi.Controllers.v2
{
    public record StationDetailsDto(
        int Id,
        string Name,
        string Location,
        int MeasurementsCount
    );

    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StationsController : ControllerBase
    {
        private readonly WaterQualityContext _context;

        public StationsController(WaterQualityContext context)
        {
            _context = context;
        }

        // GET: api/v2/stations?minMeasurements=0
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StationDetailsDto>>> GetStations(
            [FromQuery] int minMeasurements = 0)
        {
            var query = _context.Stations
                .Include(s => s.Measurements)
                .Select(s => new StationDetailsDto(
                    s.Id,
                    s.Name,
                    s.Location,
                    s.Measurements.Count
                ));

            if (minMeasurements > 0)
                query = query.Where(s => s.MeasurementsCount >= minMeasurements);

            return await query.ToListAsync();
        }
    }
}
