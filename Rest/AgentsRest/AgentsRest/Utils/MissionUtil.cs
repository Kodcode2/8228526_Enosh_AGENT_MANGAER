// Ignore Spelling: Util Utils

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
    }
}
