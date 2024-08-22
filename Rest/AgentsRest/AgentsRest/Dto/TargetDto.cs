// Ignore Spelling: Dto

using AgentsRest.Models;
using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Dto
{
    public class TargetDto
    {
        public int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public LocationModel Location { get; set; } = new LocationModel();
        public TargetStatus Status { get; set; } = TargetStatus.Live;

        public string Image { get; set; }
    }
}
