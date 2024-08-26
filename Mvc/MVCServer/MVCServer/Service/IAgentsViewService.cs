using MVCServer.ViewModels;

namespace MVCServer.Service
{
    public interface IAgentsViewService
    {
        Task<List<AgentVM>> GetAllAgents();
    }
}
