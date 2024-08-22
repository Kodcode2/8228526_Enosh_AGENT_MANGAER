// Ignore Spelling: Dto

namespace AgentsRest.Dto
{
    public class LocationDto
    {
        public string? Token { get; set; }
        public int Id { get; set; }
        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;
    }
}
