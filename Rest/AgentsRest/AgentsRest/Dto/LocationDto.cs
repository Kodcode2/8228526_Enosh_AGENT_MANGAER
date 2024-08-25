﻿// Ignore Spelling: Dto

namespace AgentsRest.Dto
{
    public class LocationDto
    {
        public string? Token { get; set; }
        public int Id { get; set; }
        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;

        public LocationDto() { }

        public LocationDto(int x, int y)
        {
            X = x;
            Y = y;
        }
        public LocationDto(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }
    }
}
