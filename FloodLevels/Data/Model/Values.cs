using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloodLevels.Data.Model
{
    public class Values
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Value { get; set; }
        public DateTime Timestamp { get; set; }
        [Required]
        public int StationId { get; set; }
        [ForeignKey("StationId")]
        public virtual Station Station { get; set; }
    }
}
