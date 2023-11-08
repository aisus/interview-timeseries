using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TimeSeriesStorage.Data.Models;
using TimeSeriesStorage.Services;

namespace TimeSeriesStorage.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsApiController : ControllerBase
    {
        private readonly IMetricsEntryService metricsEntryService;

        public MetricsApiController(IMetricsEntryService metricsEntryService)
        {
            this.metricsEntryService = metricsEntryService;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(PaginatedResponce<MetricsEntry>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPage([Required][Range(0, 999)]int limit, int page = 0)
        {
            var result = await metricsEntryService.QueryEntries(x => true, x => x.Timestamp, true, page, limit);

            return Ok(result);
        }
    }
}