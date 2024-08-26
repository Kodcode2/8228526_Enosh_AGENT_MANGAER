// Ignore Spelling: Dto

using AgentsApi.Data;
using AgentsRest.Controllers;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using static AgentsRest.Utils.ConversionModelsUtil;
using static AgentsRest.Utils.LocationUtil;

namespace AgentsRest.Service
{
    public class AgentService(
        ApplicationDbContext dbContext,
        IServiceProvider serviceProvider,
        ILogger<AgentsController> logger
    ) : IAgentService
    {
        private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();
        private IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();
        private ILocationService locationService => serviceProvider.GetRequiredService<ILocationService>();

        // Create new Agent
        public async Task<AgentModel?> CreateAgentAsync(AgentDto agentDto)
        {
            try
            {
                if (agentDto == null) { throw new ArgumentNullException(nameof(agentDto)); } // Handling null input

                AgentModel agent = AgentDtoToModel(agentDto); // Convert Dto to Model

                agent.X = -1; agent.Y = -1;
                agent.Status = AgentStatus.Inactive;

                await dbContext.AddAsync(agent); // Add Agent to DbContext
                await dbContext.SaveChangesAsync();

                return agent;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<AgentModel?> PlaceAgentAsync(int agentId, LocationDto location)
        {
            if (location == null || !IsLocationValid(location.X, location.Y)) { return null; }

            AgentModel? agent = await GetAgentByIdAsync(agentId);
            if (agent == null) return null;

            agent.X = location.X;
            agent.Y = location.Y;
            await dbContext.SaveChangesAsync();

            await missionService.GetAllMissionsAsync(); // After updating a new location, performing a possible task check
            return agent;
        }

        public async Task<AgentModel?> MoveAgentAsync(int id, string direction)
        {
            AgentModel? agent = await GetAgentByIdAsync(id);
            if (agent == null || string.IsNullOrEmpty(direction)) { return null; };

            await locationService.MoveLocationAsync(agent, direction); 

            await missionService.GetAllMissionsAsync(); // After updating a new location, performing a possible task check
            return agent;
        }

        // Get All Agents (If there are no Agents return empty list)
        public async Task<List<AgentModel>> GetAllAgentsAsync() =>
            await dbContext.Agents.ToListAsync();

        // Get Agent by id, if not exists throw error
        public async Task<AgentModel?> GetAgentByIdAsync(int id) =>
            await dbContext.Agents.FindAsync(id);

        public async Task<bool> IsAgentExistAsync(int id) =>
            await dbContext.Agents.AnyAsync(a => a.Id == id);
    }
}
