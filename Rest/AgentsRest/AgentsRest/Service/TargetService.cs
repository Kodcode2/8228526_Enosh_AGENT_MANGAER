// Ignore Spelling: Dto

using AgentsApi.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using static AgentsRest.Utils.ConversionModelsUtil;
using static AgentsRest.Utils.LocationUtil;

namespace AgentsRest.Service
{
    public class TargetService(
        ApplicationDbContext dbContext,
        IServiceProvider serviceProvider
    ) : ITargetService
    {
        private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();
        private IMissionService missionService => serviceProvider.GetRequiredService<IMissionService>();
        private ILocationService locationService => serviceProvider.GetRequiredService<ILocationService>();

        public async Task<TargetModel?> CreateTargetAsync(TargetDto targetDto)
        {
            if (targetDto == null) { return null; }

            TargetModel targetModel = TargetDtoToModel(targetDto); 

            targetModel.X = -1; targetModel.Y = -1;

            await dbContext.Targets.AddAsync(targetModel); 
            await dbContext.SaveChangesAsync();

            return targetModel;
        }

        public async Task<TargetModel?> PlaceTargetAsync(int targetId, LocationDto location)
        {
            TargetModel? target = await dbContext.Targets.FindAsync(targetId);

            if (target == null || location == null) { return null; }

            if (!IsLocationValid(location.X, location.Y))
            { throw new Exception($"Location X: {location.X}, Y: {location.Y}, not valid."); }

            target.X = location.X;
            target.Y = location.Y;
            await dbContext.SaveChangesAsync();
            return target;
        }

        public async Task<TargetModel?> MoveTargetAsync(int id, string direction)
        {
            TargetModel? target = await GetTargetByIdAsync(id);

            if (target == null) { return null; }

            await locationService.MoveLocationAsync(target, direction);

            await missionService.GetAllMissionsAsync();

            return target;
        }

        public async Task<bool> IsTargetExists(int targetId) =>
            await dbContext.Targets.AnyAsync(t => t.Id == targetId);

        // Get All Targets (If there are no Targets return empty list)
        public async Task<List<TargetModel>> GetAllTargetsAsync() =>
            await dbContext.Targets.ToListAsync();

        // Get Target by id, if not exists throw error
        public async Task<TargetModel?> GetTargetByIdAsync(int id) =>
            await dbContext.Targets.FindAsync(id) 
            ?? null;

        public async Task<bool> IsTargetExistAsync(int id) =>
            await dbContext.Targets.AnyAsync(t => t.Id == id);
    }
}
