using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Models
{
    public enum MissionStatus
    {
        Proposal,
        Assigned, 
        Completed
    }

    public class MissionModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required int AgentId { get; set; }

        [Required]
        public required int TargetId { get; set; }

        [Required]
        public required DateTime TimeLeft { get; set; }

        public DateTime? ExecutionTime { get; set; }

        public MissionStatus Status { get; set; } = MissionStatus.Proposal;

        public AgentModel? Agent { get; set; }

        public TargetModel? Target { get; set; }
    }
}
