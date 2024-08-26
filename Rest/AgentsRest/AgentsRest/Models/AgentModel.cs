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
        
        [Required, EnumDataType(typeof(AgentStatus))]
        public required AgentStatus Status { get; set; } = AgentStatus.Inactive;

        public int X { get; set; } = -1;

        public int Y { get; set; } = -1;

        public string Image { get; set; } = 
            "https://stock.adobe.com/il/images/business-male-icon-simple-flat-design-concept/519881633";

         public int Eliminations { get; set; } = 0;

        [NotMapped]
        public List<MissionModel> AgentsMissions { get; set; } = [];

    }
}
