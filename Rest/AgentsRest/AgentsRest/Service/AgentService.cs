// Ignore Spelling: Dto

using AgentsApi.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using static AgentsRest.Utils.ConversionModelsUtil;

namespace AgentsRest.Service
{
    public class AgentService(
        ApplicationDbContext dbContext
    ) : IAgentService
    {
        // Create new Agent
        public async Task<AgentModel> CreateAgentAsync(AgentDto agentDto)
        {
            if (agentDto == null) { throw new ArgumentNullException(nameof(agentDto)); } // Handling null input

            AgentModel agentModel = AgentDtoToModel(agentDto); // Convert Dto to Model

            await dbContext.AddAsync( agentModel ); // Add Agent to DbContext
            await dbContext.SaveChangesAsync();

            return agentModel;
        }

        /*// Get Agent by id, if not exists throw error
        public async Task<AgentModel> GetAgentById(int id) =>
            await dbContext.Agents.FindAsync(id)
            ?? throw new Exception($"Agent with id {id} does not exists.");*/

        public Task<AgentModel> MoveAgentAsync(string direction)
        {
            throw new NotImplementedException();
        }

        public async Task<AgentModel?> PlaceAgentAsync(int id, LocationDto locationDto)
        {
            AgentModel? agent = await GetAgentByIdAsync(id);
            LocationModel? location = LocationDtoToModel(locationDto);

            if (agent == null || location == null) { return null; }

            agent.Location = location;
            await dbContext.SaveChangesAsync();

            return agent;
        }

        public Task<AgentModel> UpdateAgentAsync(AgentDto agentDto)
        {
            throw new NotImplementedException();
        }

        // Get All Agents (If there are no Agents return empty list)
        public async Task<List<AgentModel>> GetAllAgentsAsync() =>
            await dbContext.Agents.ToListAsync();

        // Get Agent by id, if not exists throw error
        public async Task<AgentModel?> GetAgentByIdAsync(int id) =>
            await dbContext.Agents.FindAsync(id)
            ?? null;

        public async Task<bool> IsAgentExistAsync(int id) =>
            await dbContext.Agents.AnyAsync(a => a.Id == id);
    }
}
