using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface IAgentService
    {
        Task<AgentModel> CreateAgentAsync(AgentDto agentDto);
        Task<AgentModel> UpdateAgentAsync(AgentDto agentDto);
        Task<AgentModel?> PlaceAgentAsync(int id, LocationDto locationDto);
        Task<AgentModel> MoveAgentAsync(int id, string direction);
        Task<AgentModel?> GetAgentByIdAsync(int id);
        Task<List<AgentModel>> GetAllAgentsAsync();
        Task<bool> IsAgentExistAsync(int id);
    }
}
