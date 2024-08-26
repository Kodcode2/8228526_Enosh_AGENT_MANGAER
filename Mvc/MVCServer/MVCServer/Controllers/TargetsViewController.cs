using Microsoft.AspNetCore.Mvc;
using MVCServer.Service;
using MVCServer.ViewModels;

namespace MVCServer.Controllers
{
    public class TargetsViewController(
        ITargetsViewService targetsService    
    ) : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<TargetVM> targets = await targetsService.GetAllTargets();
            return View(targets);
        }
    }
}
