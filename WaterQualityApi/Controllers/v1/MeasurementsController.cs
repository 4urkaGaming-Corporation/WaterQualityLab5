using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaterQualityApi.Data;
using WaterQualityApi.Models;

namespace WaterQualityApi.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class MeasurementsController : ControllerBase
    {
        private readonly WaterQualityContext _context;

        public MeasurementsController(WaterQualityContext context)
        {
            _context = context;
        }

        // GET: api/v1/measurements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements()
        {
            return await _context.Measurements
                .Include(m => m.Station)
                .Include(m => m.Parameter)
                .ToListAsync();
        }
    }
}
