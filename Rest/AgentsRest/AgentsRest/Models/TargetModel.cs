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
        public LocationModel Location { get; set; } = new LocationModel();

        [Required, EnumDataType(typeof(TargetStatus))]
        public TargetStatus Status { get; set; } = TargetStatus.Live;

        public string Image { get; set; } =
            "https://stock.adobe.com/il/images/beard-icon-on-white-background/801737966";
    }
}
