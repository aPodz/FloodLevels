using System.ComponentModel.DataAnnotations;

namespace FloodLevels.Data.Model
{
    public class Station
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Range(1, 100)]
        public int FloodLevel { get; set; }
        [Required]
        [Range(1, 100)]
        public int DroughtLevel { get; set; }
        [Required]       
        public int TimeOutinMinutes { get; set; }
    }
}
