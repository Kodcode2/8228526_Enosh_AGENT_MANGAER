using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Models
{
    public class LocationModel
    {
        [Key]
        public int Id { get; set; }

        public int X { get; set; } = -1;

        public int Y { get; set; } = -1;
    }
}