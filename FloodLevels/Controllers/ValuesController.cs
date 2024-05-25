using FloodLevels.Data.Model;
using FloodLevels.Data;
using Microsoft.AspNetCore.Mvc;
using vosplzen.sem2._2023.apiClient.Contracts;

namespace FloodLevels.Controllers
{   
    [ApiController]
    [Route("api/floodvalues")]
    public class FloodValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FloodValuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostFloodValue([FromBody] FloodReport floodReport)
        {
            if (floodReport == null)
            {               
                return BadRequest();
            }
            Console.WriteLine(floodReport.TimeStamp);
            var value = new Values
            {
                StationId = floodReport.StationId,
                Timestamp = floodReport.TimeStamp,
                Value = floodReport.Value
            };

            _context.Values.Add(value);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
