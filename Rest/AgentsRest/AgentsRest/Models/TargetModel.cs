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

        [Required, EnumDataType(typeof(TargetStatus))]
        public required TargetStatus Status { get; set; } = TargetStatus.Live;

        public int X { get; set; } = -1;

        public int Y { get; set; } = -1;

        [Required]
        public required bool IsDetected { get; set; } = false;

        public string Image { get; set; } =
            "https://stock.adobe.com/il/images/beard-icon-on-white-background/801737966";
    }
}
