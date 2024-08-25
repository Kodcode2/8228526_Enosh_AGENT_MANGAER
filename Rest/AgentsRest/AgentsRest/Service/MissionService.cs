using AgentsRest.Models;
using static AgentsRest.Utils.MissionUtil;
using static AgentsRest.Utils.ConversionModelsUtil;
using Accord.Collections;
using AgentsApi.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AgentsRest.Dto;
using System.Reflection;

namespace AgentsRest.Service
{
    public class MissionService(
        ApplicationDbContext dbContext,
        IServiceProvider serviceProvider
    ) : IMissionService
    {
        private IAgentService agentService = serviceProvider.GetRequiredService<IAgentService>();
        private ITargetService targetService = serviceProvider.GetRequiredService<ITargetService>();
        private ILocationService locationService = serviceProvider.GetRequiredService<ILocationService>();

        public async Task<List<MissionModel>> GetAllMissionsAsync()
        {
            List<MissionModel> possibleMissions = await FindPossibleMissionsAsync();

            if (possibleMissions == null || !possibleMissions.Any()) { return new List<MissionModel>(); }

            var existingMissions = await dbContext.Missions
                .Select(m => new { m.AgentId, m.TargetId })
                .ToListAsync();

            var existingMissionsSet = new HashSet<(int AgentId, int TargetId)>(
                existingMissions.Select(m => (m.AgentId, m.TargetId))
            );

            var newMissions = possibleMissions
                .Where(m => !existingMissionsSet.Contains((m.AgentId, m.TargetId)))
                .ToList();

            if (newMissions.Any())
            {
                await dbContext.Missions.AddRangeAsync(newMissions);
                await dbContext.SaveChangesAsync();
            }

            return await dbContext.Missions.ToListAsync();
        }

        public async Task<List<MissionModel>> FindPossibleMissionsAsync()
        {
            try
            {
                List<MissionModel> missions = new ();

                List<AgentModel> agents = await dbContext.Agents.Where(a => a.Status == AgentStatus.Inactive).ToListAsync();

                if (agents == null || agents.Count == 0) { throw new Exception("Agents is null or empty."); }

                List<PointWithIdModel> pointsAgents = AgentsToPoints(agents);

                List<TargetModel> targets = await dbContext.Targets.Where(t => t.Status == TargetStatus.Live).ToListAsync();

                if (targets == null || targets.Count == 0) { throw new Exception("Targets is null or empty."); }

                KDTree<PointWithIdModel> agentsTree = GenerateDormantAgentKDTree(pointsAgents);

                if (agentsTree == null || agentsTree.Count == 0) { throw new Exception("AgentsTree is null or empty."); }


                foreach (var target in targets)
                {
                    Dictionary<int, double> potentialMissions = FindAgentPerTarget(agentsTree, LocationModelToDto(target.Location));
                    foreach (var kvp in potentialMissions)
                    {
                        int agentId = kvp.Key;
                        double distance = kvp.Value;
                        var mission = CreateMission(target.Id, agentId, distance);
                        missions.Add(mission);
                    }
                }
                return missions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public KDTree<PointWithIdModel> GenerateDormantAgentKDTree(List<PointWithIdModel> pointsAgents)
        {
            KDTree<PointWithIdModel> agentsTree = new KDTree<PointWithIdModel>(dimensions: 2);

            foreach (var item in pointsAgents)
            {
                agentsTree.Add(position: item.Coordinates, value: item);
            }
            return agentsTree;
        }

        public Dictionary<int, double> FindAgentPerTarget(KDTree<PointWithIdModel> agentsTree, LocationDto location)
        {
            double[] targetQuery = { location.X, location.Y };

            double maxDistance = 200;

            var nearestAgents = agentsTree.Nearest(
                position: targetQuery, 
                radius: maxDistance, 
                maximum: int.MaxValue
            );

            return nearestAgents
                .OrderBy(x => x.Distance)
                .ToDictionary(
                    n => n.Node.Value.AgentId,
                    n => n.Distance
                );
        }

        public List<PointWithIdModel> AgentsToPoints(List<AgentModel> agents) =>
            agents.Select(AgentToPointWithId).ToList();

        public MissionModel CreateMission(int targetId, int agentId, double distance) =>
            new MissionModel()
            {
                AgentId = agentId,
                TargetId = targetId,
                Distance = distance
            };
    }
}

