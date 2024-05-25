using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using FloodLevels.Data.Model;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace FloodLevels.Pages.StationPages
{
    public class ValuesOverviewModel : PageModel
    {
        public List<ValueInfo> Values = new List<ValueInfo>();
        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }
        public void OnGet()
        {
            try
            {
                string connectionString = "Server=(localdb)\\mssqllocaldb;Database=FloodLevels;Trusted_Connection=True;MultipleActiveResultSets=true";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                                    SELECT 
                                        V.Id, 
                                        V.Value, 
                                        V.Timestamp, 
                                        S.Title AS StationTitle,
                                        S.FloodLevel,
                                        S.DroughtLevel
                                    FROM 
                                        [Values] V
                                    INNER JOIN 
                                        Stations S 
                                    ON 
                                        V.StationId = S.Id";

                    if (!string.IsNullOrEmpty(Filter))
                    {
                        sql += " WHERE S.Title LIKE @Filter";
                    }
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (!string.IsNullOrEmpty(Filter))
                        {
                            command.Parameters.AddWithValue("@Filter", $"%{Filter}%");
                        }
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Values = new List<ValueInfo>();
                            while (reader.Read())
                            {
                                ValueInfo newValue = new ValueInfo();
                                newValue.Id = reader.GetInt32(0);
                                newValue.Value = reader.GetInt32(1);
                                newValue.Timestamp = reader.GetDateTime(2);
                                newValue.StationTitle = reader.GetString(3);
                                newValue.FloodLevel = reader.GetInt32(4);
                                newValue.DroughtLevel = reader.GetInt32(5);
                                Values.Add(newValue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba: {ex.Message}");
            }
        }
    }

    public class ValueInfo
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime Timestamp { get; set; }
        public string StationTitle { get; set; }
        public int FloodLevel { get; set; }
        public int DroughtLevel { get; set; }
    }
}
