using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Models
{
    public class EstimatedDurationsModel 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required int MissionId { get; set; }

        [Required]
        public required double EstimatedDuration { get; set; }

        public MissionModel Mission { get; set; }
    }
}
