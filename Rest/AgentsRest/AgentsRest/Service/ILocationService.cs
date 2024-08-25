using AgentsRest.Models;

namespace AgentsRest.Service
{
    public interface ILocationService
    {
        Task<AgentModel?> MoveLocationAsync(AgentModel agent, string direction);
        Task<TargetModel?> MoveLocationAsync(TargetModel target, string direction);
    }
}
