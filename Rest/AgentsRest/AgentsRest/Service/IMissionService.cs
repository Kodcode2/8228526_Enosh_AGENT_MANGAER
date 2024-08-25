using Accord.Collections;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.SqlServer.Server;

namespace AgentsRest.Service
{
    public interface IMissionService
    {
        Task<List<MissionModel>> GetAllMissionsAsync();
    }
}
