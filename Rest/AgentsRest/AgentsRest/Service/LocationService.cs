using AgentsApi.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.Extensions.DependencyInjection;
using static AgentsRest.Utils.LocationUtil;
using static AgentsRest.Utils.ConversionModelsUtil;
using System.Reflection;

namespace AgentsRest.Service
{
    public class LocationService(
        ApplicationDbContext dbContext,
        IServiceProvider serviceProvider
    ) : ILocationService
    {
        private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();
        private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();
        private IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();

        public async Task<AgentModel?> MoveLocationAsync(AgentModel agent, string direction)
        {
            if (agent == null || direction == null) 
            { throw new Exception("Invalid input."); }
            LocationModel cuurentLocation = new() { X = agent.X, Y = agent.Y };

            LocationModel? newLocation = GetMove(cuurentLocation, direction);

            if (newLocation == null) { throw new Exception("Invalid new location."); }

            AgentModel? agentModel = await dbContext.Agents.FindAsync(agent.Id);

            agentModel.X += newLocation.X;
            agentModel.Y += newLocation.Y;
            await dbContext.SaveChangesAsync();

            return agent;
        }

        public async Task<TargetModel?> MoveLocationAsync(TargetModel target, string direction)
        {
            if (target == null || direction == null) 
            { throw new Exception("Invalid input."); } 

            LocationModel cuurentLocation = new() { X = target.X, Y = target.Y };

            LocationModel? newLocation = GetMove(cuurentLocation, direction);

            if (newLocation == null) { throw new Exception("Invalid new location."); }

            target.X += newLocation.X;
            target.Y += newLocation.Y;
            await dbContext.SaveChangesAsync();

            return target;
        }
    }
}
