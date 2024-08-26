using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface ILocationService
    {
        Task<T?> MoveLocationAsync<T>(T location, string direction) where T : class, ILocationModel;
    }
}
