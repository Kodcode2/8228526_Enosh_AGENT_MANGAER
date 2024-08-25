using Accord.Collections;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.SqlServer.Server;

namespace AgentsRest.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMissionsAsync();
        Task<List<MissionModel>> UpdateAllMissionsAsync();
        Task<MissionModel?> GetMissionByIdAsync(int missionId);
        Task<MissionModel?> AllocateMissionAsync(int missionId);
        Task<bool> IsMissionExistAsync(int id);
    }
}
