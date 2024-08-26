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

        public static LocationModel? GetMove(LocationModel currentLocation, string direction)
        {            
            Dictionary<string, Func<LocationModel, (int x , int y)>> map = new() 
            {
                {  "e", (location) => (0, 1) },
                {  "w", (location) => (0, -1) },
                {  "s", (location) => (1, 1) },
                {  "n", (location) => (-1, 1) },
                {  "nw", (location) => (-1, -1) },
                {  "ne", (location) => (-1, 1) },
                {  "sw", (location) => (1, -1) },
                {  "se", (location) => (1, 1) },
                {  "wn", (location) => (-1, -1) },
                {  "en", (location) => (-1, 1) },
                {  "ws", (location) => (1, -1) },
                {  "es", (location) => (1, 1) },
            };

            var (x, y) = map[direction](currentLocation);

            return 
                IsLocationValid(currentLocation.X + x, currentLocation.Y + y) ?
                new LocationModel() 
                {
                    X = currentLocation.X + x,
                    Y = currentLocation.Y + y
                } : null;
        }
    }
}
