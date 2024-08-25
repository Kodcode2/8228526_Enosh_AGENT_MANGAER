using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface ITargetService
    {
        Task<TargetModel?> CreateTargetAsync(TargetDto target);
        Task<TargetModel?> PlaceTargetAsync(int targetId, LocationDto location);
        Task<TargetModel> MoveTargetAsync(string direction);
        Task<List<TargetModel>> GetAllTargetsAsync();
        Task<TargetModel?> GetTargetByIdAsync(int id);
        Task<bool> IsTargetExistAsync(int id);
    }
}
