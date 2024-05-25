using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using FloodLevels.Data; // Zahrnuje vaše DbContext a modely
using FloodLevels.Data.Model;

namespace FloodLevels.Pages.StationPages
{
    public class OverviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OverviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int ErrorCount { get; set; }
        public Dictionary<DateTime, int> ErrorData { get; set; }
        public int TodaysErrorCount { get; set; }
        public int ThisMonthsErrorCount { get; set; }
        public int ThisYearsErrorCount { get; set; }
        public List<Station> TodaysStationErrors { get; set; }
        public List<Station> StationsWithMissingData { get; set; }

        public List<Station> GetStationsWithMissingData(List<Station> allStations)
        {
            var stationsWithMissingData = new List<Station>();

            foreach (var station in allStations)
            {
                var lastRecord = _context.Values
                    .Where(v => v.StationId == station.Id)
                    .OrderByDescending(v => v.Timestamp)
                    .FirstOrDefault();

                if (lastRecord == null || (DateTime.UtcNow - lastRecord.Timestamp).TotalMinutes > station.TimeOutinMinutes)
                {
                    stationsWithMissingData.Add(station);
                }
            }

            return stationsWithMissingData;
        }
        public void CountExceededValues(List<Station> allStations, List<Values> allValues, DateTime today)
        {
            ErrorCount = 0;
            ErrorData = new Dictionary<DateTime, int>();
            TodaysErrorCount = 0;
            TodaysStationErrors = new List<Station>();

            var thisMonth = new DateTime(today.Year, today.Month, 1);
            var thisYear = new DateTime(today.Year, 1, 1);

            foreach (var value in allValues)
            {
                var station = allStations.FirstOrDefault(s => s.Id == value.StationId);

                if (station != null)
                {
                    var floodLevel = station.FloodLevel;
                    var droughtLevel = station.DroughtLevel;

                    if (value.Value > floodLevel || value.Value < droughtLevel)
                    {
                        ErrorCount++;

                        var date = value.Timestamp.Date;

                        if (ErrorData.ContainsKey(date))
                        {
                            ErrorData[date]++;
                        }
                        else
                        {
                            ErrorData[date] = 1;
                        }

                        if (date == today)
                        {
                            TodaysErrorCount++;

                            TodaysStationErrors.Add(station);
                        }

                        if (date.Year == today.Year && date.Month == today.Month)
                        {
                            ThisMonthsErrorCount++;
                        }

                        if (date.Year == today.Year)
                        {
                            ThisYearsErrorCount++;
                        }
                    }
                }
            }
        }


        public IActionResult OnGet()
        {
            try
            {
                var allStations = _context.Stations.ToList();
                var allValues = _context.Values.ToList();
                var today = DateTime.Today;
                StationsWithMissingData = GetStationsWithMissingData(allStations);
                CountExceededValues(allStations, allValues, today);

                return Page();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
