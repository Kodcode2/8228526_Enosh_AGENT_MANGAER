// Ignore Spelling: Utils Util

using AgentsRest.Dto;
using AgentsRest.Models;
using System;

namespace AgentsRest.Utils
{
    public class LocationUtil
    {
        public static bool IsLocationValid(LocationDto location) => 
            location.X > 0 
            && location.X < 1000
            && location.Y > 0
            && location.Y < 1000
            ? true : false;

        public static bool IsLocationValid(LocationModel location) =>
            location.X > 0
            && location.X < 1000
            && location.Y > 0
            && location.Y < 1000
            ? true : false;

        public static bool IsLocationValid(int x, int y) =>
            x > 0
            && x < 1000
            && y > 0
            && y < 1000
            ? true : false;

        public static LocationDto? GetNewLocation(LocationDto currentLocation, string direction)
        {
            var (x, y) = (currentLocation.X, currentLocation.Y);

            (x, y) = direction switch
            {
                "e" => (x, y++),
                "w" => (x, y--),
                "s" => (x++, y),
                "n" => (x--, y),
                "nw" => (x--, y--), "wn" => (x--, y--),
                "ne" => (x--, y++), "en" => (x--, y++),
                "sw" => (x++, y--), "ws" => (x++, y--),
                "se" => (x++, y++), "es" => (x++, y++),
                _ => throw new Exception("Invalid command.")
            };

            return IsLocationValid(x, y) ? new LocationDto(x, y) : null;
        }
    }
}
