using Microsoft.AspNetCore.Mvc;
using MVCServer.Service;
using MVCServer.ViewModels;

namespace MVCServer.Controllers
{
    public class AgentsViewController(
        IAgentsViewService agentsService    
    ) : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<AgentVM> agents = await agentsService.GetAllAgents();
            return View(agents);
        }
    }
}
