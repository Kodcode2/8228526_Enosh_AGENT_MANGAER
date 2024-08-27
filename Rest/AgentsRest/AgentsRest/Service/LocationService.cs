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

        public async Task<T?> MoveLocationAsync<T>(T location, string direction) where T : class, ILocationModel
        {
            if (location == null || direction == null)
            {
                return default;
            }

            LocationModel currentLocation = new() { X = location.X, Y = location.Y };
            LocationModel? newLocation = GetMove(currentLocation, direction);

            if (newLocation == null)
            {
                return null;
            }

            location.X = newLocation.X;
            location.Y = newLocation.Y;
            await dbContext.SaveChangesAsync();

            return location;
        }
    }
}
