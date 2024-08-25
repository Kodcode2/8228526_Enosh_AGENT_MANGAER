using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Models
{
    public class EstimatesMissionsTimeModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required int MissionId { get; set; }

        public MissionModel Mission { get; set; }

        [Required]
        public required DateTime LeftTime { get; set; }
    }
}
