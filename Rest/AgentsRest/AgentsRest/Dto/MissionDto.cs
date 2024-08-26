// Ignore Spelling: Dto

using AgentsRest.Models;
using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Dto
{
    public class MissionDto
    {
        public string? Token { get; set; }
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int TargetId { get; set; }
        public double Distance { get; set; }
        public DateTime StartTime { get; set; }
        public double EstimatedDuration { get; set; }
        public DateTime? ExecutionTime { get; set; }
        public MissionStatus Status { get; set; } 
    }
}
