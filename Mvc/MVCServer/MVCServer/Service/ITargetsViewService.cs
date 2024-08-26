using MVCServer.ViewModels;

namespace MVCServer.Service
{
    public interface ITargetsViewService
    {
        Task<List<TargetVM>> GetAllTargets();
    }
}
