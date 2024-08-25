// Ignore Spelling: Dto

using AgentsApi.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using static AgentsRest.Utils.ConversionModelsUtil;

namespace AgentsRest.Service
{
    public class TargetService(
        ApplicationDbContext dbContext
    ) : ITargetService
    {
        // Creating new Target
        public async Task<TargetModel> CreateTargetAsync(TargetDto targetDto)
        {
            if (targetDto == null ) { throw new ArgumentNullException(nameof(targetDto)); } // Handling null input

            TargetModel targetModel = TargetDtoToModel(targetDto); // Convert Dto to Model

            await dbContext.Targets.AddAsync( targetModel ); // Add Target to DbContext
            await dbContext.SaveChangesAsync();

            return targetModel;
        }

        public Task<TargetModel?> MoveTargetAsync(string direction)
        {
            throw new NotImplementedException();
        }

        public Task<TargetModel?> PlaceTargetAsync(int targetId, LocationDto location)
        {
            if (location == null ) { throw new ArgumentNullException(nameof(location)); } // Handling null input
            throw new NotImplementedException();
        }

        public async Task<bool> IsTargetExists(int targetId) =>
            await dbContext.Targets.FindAsync(targetId)
            ?? false;

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
