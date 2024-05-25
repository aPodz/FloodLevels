using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FloodLevels.Data;
using FloodLevels.Data.Model;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace FloodLevels.Pages.StationPages
{
    public class DetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DetailModel> _logger;
        public DetailModel(ApplicationDbContext context, ILogger<DetailModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Station Station { get; set; }

        [BindProperty]
        public bool IsEditMode { get; set; }
        public List<Values> StationValues { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Station = await _context.Stations.FindAsync(id);

            if (Station == null)
            {
                return NotFound();
            }

            StationValues = await _context.Values
                .Where(v => v.StationId == id)
                .OrderByDescending(v => v.Timestamp)
                .ToListAsync();

            IsEditMode = false;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            

            var stationToUpdate = await _context.Stations.FindAsync(Station.Id);

            if (stationToUpdate == null)
            {
                return NotFound();
            }

            stationToUpdate.Title = Station.Title;
            stationToUpdate.FloodLevel = Station.FloodLevel;
            stationToUpdate.DroughtLevel = Station.DroughtLevel;
            stationToUpdate.TimeOutinMinutes = Station.TimeOutinMinutes;

            if (_context.Stations.Any(s => s.Title == Station.Title && s.Id != Station.Id))
            {
                return Content("<script>alert('This station already exists.');window.history.back();</script>", "text/html");
            }

            if (stationToUpdate.DroughtLevel >= stationToUpdate.FloodLevel)
            {
                return Content("<script>alert('Flood level must be higher than Drought level');window.history.back();</script>", "text/html");
            }

            await _context.SaveChangesAsync();

            IsEditMode = false;
            return RedirectToPage("./Detail", new { id = Station.Id });

        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            Station = await _context.Stations.FindAsync(id);

            if (Station == null)
            {
                return NotFound();
            }

            IsEditMode = true;
            
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var stationToDelete = await _context.Stations.FindAsync(id);

            if (stationToDelete == null)
            {
                return NotFound();
            }

            var valuesToDelete = await _context.Values.Where(v => v.StationId == id).ToListAsync();
            _context.Values.RemoveRange(valuesToDelete);
            _context.Stations.Remove(stationToDelete);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Stations");
        }
    }
}
