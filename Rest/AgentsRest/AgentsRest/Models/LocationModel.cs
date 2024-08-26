using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Models
{
    public class LocationModel : ILocationModel
    {
        public int X { get; set; } 
        public int Y { get; set; } 
    }
}