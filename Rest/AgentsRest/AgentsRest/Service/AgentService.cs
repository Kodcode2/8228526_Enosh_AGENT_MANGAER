// Ignore Spelling: Dto

using AgentsApi.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using static AgentsRest.Utils.ConversionModelsUtil;
using static AgentsRest.Utils.LocationUtil;

namespace AgentsRest.Service
{
    public class AgentService(
        ApplicationDbContext dbContext,
        IServiceProvider serviceProvider
    ) : IAgentService
    {
        private ITargetService targetService = serviceProvider.GetRequiredService<ITargetService>();
        private IMissionService missionService = serviceProvider.GetRequiredService<IMissionService>();
        private ILocationService locationService = serviceProvider.GetRequiredService<ILocationService>();

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

        public async Task<AgentModel?> PlaceAgentAsync(int agentId, LocationDto location)
        {
            if (location == null) { throw new ArgumentNullException(nameof(location)); } // Handling null input

            AgentModel? agent = await dbContext.Agents.FindAsync(agentId);

            if (agent == null) { return null; }

            if (!IsLocationValid(location.X, location.Y))
            { throw new Exception($"Location X: {location.X}, Y: {location.Y}, not valid."); }

            agent.Location.X = location.X;
            agent.Location.Y = location.Y;
            await dbContext.SaveChangesAsync();
            return agent;
        }

        public async Task<AgentModel?> MoveAgentAsync(int id, string direction) =>
            await locationService.MoveLocationAsync(
                await GetAgentByIdAsync(id) ?? throw new Exception("Agent not found."),
                direction
            );

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
