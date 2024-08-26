// Ignore Spelling: Dto

namespace MVCServer.Dto
{
    public enum TargetStatus
    {
        Live,
        Dead
    }

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
