// Ignore Spelling: Util Utils

using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Utils
{
    public class MissionUtil
    {
        public static PointWithIdModel AgentToPointWithId(AgentModel agentModel) =>
            new PointWithIdModel()
            {
                Coordinates = [agentModel.Location.X, agentModel.Location.Y],
                AgentId = agentModel.Id,
            };

        public static double ComputeTimeLeft(double distance) => distance / 5;
    }
}
