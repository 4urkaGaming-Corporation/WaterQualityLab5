using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterQualityApi.Data;

namespace WaterQualityApi.Controllers.v2
{
    public record MeasurementDto(
        int Id,
        int StationId,
        string StationName,
        int ParameterId,
        string ParameterName,
        string Unit,
        DateTime MeasuredAt,
        double Value
    );

    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MeasurementsController : ControllerBase
    {
        private readonly WaterQualityContext _context;

        public MeasurementsController(WaterQualityContext context)
        {
            _context = context;
        }

        // GET: api/v2/measurements?stationId=1&page=1&pageSize=20
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeasurementDto>>> GetMeasurements(
            [FromQuery] int? stationId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("page and pageSize must be positive.");

            var query = _context.Measurements
                .Include(m => m.Station)
                .Include(m => m.Parameter)
                .AsQueryable();

            if (stationId.HasValue)
                query = query.Where(m => m.StationId == stationId.Value);

            query = query.OrderByDescending(m => m.MeasuredAt);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MeasurementDto(
                    m.Id,
                    m.StationId,
                    m.Station.Name,
                    m.ParameterId,
                    m.Parameter.Name,
                    m.Parameter.Unit,
                    m.MeasuredAt,
                    m.Value
                ))
                .ToListAsync();

            return items;
        }
    }
}
