using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgentsRest.Models
{
    public enum AgentStatus
    {
        Active,
        Inactive
    }

    public class AgentModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Nickname { get; set; } = string.Empty;

        [Required]
        public required LocationModel Location { get; set; } = new LocationModel();
        
        [Required, EnumDataType(typeof(AgentStatus))]
        public required AgentStatus Status { get; set; } = AgentStatus.Inactive;

        public string Image { get; set; } = 
            "https://stock.adobe.com/il/images/business-male-icon-simple-flat-design-concept/519881633";

        [NotMapped]
        public List<MissionModel> AgentsMissions { get; set; } = [];

    }
}
