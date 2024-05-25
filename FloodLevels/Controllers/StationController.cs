using FloodLevels.Data.Model;
using FloodLevels.Data;
using Microsoft.AspNetCore.Mvc;



namespace FloodLevels.Controllers
{
    [ApiController]
    [Route("api/")]
    public class StationController : Controller
    {
        public IActionResult StationOverview()
        {
            return View();
        }

        private ApplicationDbContext _context { get; set; }

        public StationController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("get-stations")]
        public IActionResult GetListOfStations()
        {
            var list = _context.Stations.ToList();
            return StatusCode(200, new JsonResult(list));
        }
        [HttpPost]
        [Route("add-station")]
        public IActionResult AddStation(Station Station)
        {
            if (_context.Stations.Where(x => x.Title.Equals(Station.Title)).Any())
            {
                return StatusCode(400,
                    new JsonResult("Duplicities are not allowed."));
            }
            _context.Stations.Add(Station);
            _context.SaveChanges();

            return StatusCode(200,
                new JsonResult("Station has been successfully added"));

        }

        
    }
}
