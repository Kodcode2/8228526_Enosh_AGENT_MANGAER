// Ignore Spelling: Dto

using AgentsRest.Models;
using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Dto
{
    public class TargetDto
    {
        public string? Token { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; } 
        public TargetStatus Status { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsDetected { get; set; }
    }
}
