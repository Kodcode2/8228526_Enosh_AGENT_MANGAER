using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Models
{
    public enum TargetStatus
    {
        Live,
        Dead
    }

    public class TargetModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        [Required]
        public required LocationModel Location { get; set; } = new LocationModel();

        [Required, EnumDataType(typeof(TargetStatus))]
        public required TargetStatus Status { get; set; } = TargetStatus.Live;

        [Required]
        public required bool IsDetected { get; set; } = false;

        public string Image { get; set; } =
            "https://stock.adobe.com/il/images/beard-icon-on-white-background/801737966";
    }
}
