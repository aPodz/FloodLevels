using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FloodLevels.Data;
using FloodLevels.Data.Model;

namespace FloodLevels.Pages.StationPages
{
    public class CreateStationModel : PageModel
    {

        [BindProperty]
        public Station Station { get; set; }

        private ApplicationDbContext _context;
        public CreateStationModel(ApplicationDbContext context)
        {
            Station = new Station();
            _context = context;
        }
        public IActionResult OnPost()
        {
            if (_context.Stations.Any(s => s.Title == Station.Title))
            {
                return Content("<script>alert('This station already exists.');window.history.back();</script>", "text/html");
            }

            _context.Stations.Add(Station);
            _context.SaveChanges();

            return RedirectToPage("./Stations");
        }


        public void OnGet()
        {
        }
    }
}
