using Microsoft.AspNetCore.Mvc;
using MVCServer.Service;
using MVCServer.ViewModels;

namespace MVCServer.Controllers
{
    public class MissionsViewController(
        IMissionsViewService missionsService    
    ) : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<MissionVM> missions = await missionsService.GetAllMissions();
            return View(missions);
        }

        public async Task<IActionResult> AssignMission(int id)
        {
            MissionVM mission = await missionsService.AssignMission(id);
            return View(mission);
        }
    }
}
