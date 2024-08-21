using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Models
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nickname { get; set; } = string.Empty;

        

    }
}
