using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgentsRest.Models
{
    public enum MissionStatus
    {
        Proposal,
        Assigned, 
        Completed,
        Canceled
    }

    public class MissionModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required int AgentId { get; set; }

        [Required]
        public required int TargetId { get; set; }

        public double Distance { get; set; }

        public DateTime StartTime { get; set; } 

        public double EstimatedDuration { get; set; }

        public DateTime? ExecutionTime { get; set; }

        public MissionStatus Status { get; set; } = MissionStatus.Proposal;

        public AgentModel? Agent { get; set; }

        public TargetModel? Target { get; set; }

        [NotMapped]
        public List<EstimatedDurationsModel> HistoryTimeLeft { get; set; } = [];
    }
}
