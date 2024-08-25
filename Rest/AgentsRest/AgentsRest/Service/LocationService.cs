using AgentsApi.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.Extensions.DependencyInjection;
using static AgentsRest.Utils.LocationUtil;
using static AgentsRest.Utils.ConversionModelsUtil;

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

            LocationDto cuurentLocation = LocationModelToDto(agent.Location);

            LocationDto? newLocation = GetNewLocation(cuurentLocation, direction);

            if (newLocation == null) { throw new Exception("Invalid new location."); }

            agent.Location.X = newLocation.X;
            agent.Location.Y = newLocation.Y;
            await dbContext.SaveChangesAsync();

            return agent;
        }

        public async Task<TargetModel?> MoveLocationAsync(TargetModel target, string direction)
        {
            if (target == null || direction == null) 
            { throw new Exception("Invalid input."); } 

            LocationDto cuurentLocation = LocationModelToDto(target.Location);

            LocationDto? newLocation = GetNewLocation(cuurentLocation, direction);

            if (newLocation == null) { throw new Exception("Invalid new location."); }

            target.Location.X = newLocation.X;
            target.Location.Y = newLocation.Y;
            await dbContext.SaveChangesAsync();

            return target;
        }
    }
}
