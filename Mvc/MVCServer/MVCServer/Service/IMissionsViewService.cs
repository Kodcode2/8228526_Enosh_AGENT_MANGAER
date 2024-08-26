using MVCServer.ViewModels;

namespace MVCServer.Service
{
    public interface IMissionsViewService
    {
        Task<List<MissionVM>> GetAllMissions();
        Task<MissionVM> AssignMission(int id);
    }
}
