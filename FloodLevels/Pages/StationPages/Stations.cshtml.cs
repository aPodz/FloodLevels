using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using FloodLevels.Data.Model;
using Microsoft.EntityFrameworkCore;
using FloodLevels.Data;
using System.Linq;
using System.Collections.Generic;


namespace FloodLevels.Pages.StationPages
{
    public class StationsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public StationsModel(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public List<StationInfo> Stations = new List<StationInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Server=(localdb)\\mssqllocaldb;Database=FloodLevels;Trusted_Connection=True;MultipleActiveResultSets=true";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Stations";
                   
                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                StationInfo station = new StationInfo();
                                station.Id = reader.GetInt32(0);
                                station.Title = reader.GetString(1);
                                station.FloodLevel = reader.GetInt32(2);
                                station.DroughtLevel = reader.GetInt32(3);
                                station.TimeOutinMinutes = reader.GetInt32(4);

                                Stations.Add(station);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public class StationInfo
        {
            [Key]
            public int Id { get; set; }

            public string Title { get; set; }

            public int FloodLevel { get; set; }

            public int DroughtLevel { get; set; }

            public int TimeOutinMinutes { get; set; }
        }
    }
}
