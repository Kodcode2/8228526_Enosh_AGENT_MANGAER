// Ignore Spelling: Dto

using AgentsRest.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Dto
{
    public class AgentDto
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public LocationDto Location { get; set; } = new LocationDto();
        public AgentStatus Status { get; set; } = AgentStatus.Inactive;
        public string Image { get; set; } = string.Empty;
        public List<MissionModel> AgentsMissions { get; set; } = [];
    }
}
