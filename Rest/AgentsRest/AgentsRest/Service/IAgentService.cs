using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface IAgentService
    {
        Task<AgentModel> CreateAgentAsync(AgentModel agentModel);

        Task<AgentModel> UpdateAgentAsync(AgentModel agentModel);

        Task<AgentModel> PlaceAgentAsync(LocationModel locationModel);

        Task<AgentModel> MoveAgentAsync(string direction);
    }
}
