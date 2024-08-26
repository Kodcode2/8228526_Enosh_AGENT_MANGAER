// Ignore Spelling: Util Utils

using Accord.Math;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Utils
{
    public class MissionUtil
    {
        public static PointWithIdModel AgentToPointWithId(AgentModel agentModel) =>
            new PointWithIdModel()
            {
                Coordinates = [agentModel.X, agentModel.Y],
                AgentId = agentModel.Id,
            };

        public static double ComputeTimeLeft(double distance) => distance / 5;

        public static bool IsEliminated(LocationModel targetLocation, LocationModel agentLocation) =>
            agentLocation.X == targetLocation.X && agentLocation.Y == targetLocation.Y
            ? true : false;

        public static double ComputeDistance(int x1, int y1, int x2, int y2) =>
            Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

        public static (int x, int y) MovePointTowards(int x1, int y1, int x2, int y2)
        {
            double distance = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
            if (distance == 0) return (x2, y2);

            return ((int)Math.Round(x2 + (x1 - x2) / distance),
                    (int)Math.Round(y2 + (y1 - y2) / distance));
        }

        public static (double x, double y) MovePointTowards(double x1, double y1, double x2, double y2)
        {
            double distance = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
            if (distance == 0) return (x2, y2);

            return (x2 + (x1 - x2) / distance, y2 + (y1 - y2) / distance);
        }
    }
}
