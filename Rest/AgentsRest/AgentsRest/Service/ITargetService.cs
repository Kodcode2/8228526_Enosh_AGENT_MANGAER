using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface ITargetService
    {
        Task<TargetModel> CreateTargetAsync(TargetModel target);
        Task<List<TargetModel>> GetTargetsAsync();
        Task<TargetModel> PlaceTargetAsync(LocationModel location);
        Task<TargetModel> MoveTargetAsync(string direction);
    }
}
