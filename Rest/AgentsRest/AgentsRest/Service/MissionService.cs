using AgentsRest.Models;
using KdTree.Math;
using KdTree;

namespace AgentsRest.Service
{
    public class MissionService(
        IServiceProvider serviceProvider    
    ) : IMissionService
    {
        private IAgentService agentService = serviceProvider.GetRequiredService<IAgentService>();
        private ITargetService targetService = serviceProvider.GetRequiredService<ITargetService>();

        public void InitKdTreeSingleton()
        {

        }
    }
}
