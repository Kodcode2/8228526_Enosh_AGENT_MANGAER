using AgentsRest.Models;
using static AgentsRest.Utils.MissionUtil;
using static AgentsRest.Utils.LocationUtil;
using static AgentsRest.Utils.ConversionModelsUtil;
using Accord.Collections;
using AgentsApi.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AgentsRest.Dto;
using System.Reflection;
using Accord.MachineLearning.Clustering;
using AgentsRest.Controllers;

namespace AgentsRest.Service
{
    public class MissionService(
        ApplicationDbContext dbContext,
        IServiceProvider serviceProvider,
        ILogger<MissionStatus> logger
    ) : IMissionService
    {
        private IAgentService agentService => serviceProvider.GetRequiredService<IAgentService>();
        private ITargetService targetService => serviceProvider.GetRequiredService<ITargetService>();
        private ILocationService locationService => serviceProvider.GetRequiredService<ILocationService>();

        public async Task<MissionModel?> GetMissionByIdAsync(int missionId)
        {
            try
            {
                MissionModel? mission = await dbContext.Missions.FindAsync(missionId);

                if (mission == null) { throw new Exception("Mission not exists."); }

                return mission;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<MissionModel>> GetAllMissionsAsync()
        {
            List<MissionModel> possibleMissions = await FindPossibleMissionsAsync();
            if (possibleMissions == null || !possibleMissions.Any())
            { return new List<MissionModel>(); }

            var existingMissionsList = await dbContext.Missions
                .Select(m => new { m.AgentId, m.TargetId })
                .ToListAsync();

            var existingMissionsSet = new HashSet<(int AgentId, int TargetId)>(
                existingMissionsList.Select(m => (m.AgentId, m.TargetId))
            );

            List<MissionModel> newMissions = possibleMissions
                .Where(m => !existingMissionsSet.Contains((m.AgentId, m.TargetId)))
                .ToList();

            if (newMissions.Any())
            {
                await dbContext.Missions.AddRangeAsync(newMissions);
                await dbContext.SaveChangesAsync();
            }

            return await dbContext.Missions.ToListAsync();
        }

        /*public async Task<List<MissionModel>> GetAllMissionsAsync()
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
        }*/

        public async Task<List<MissionModel>> FindPossibleMissionsAsync()
        {
            try
            {
                List<AgentModel> agents = await dbContext.Agents
                    .Where(a => a.Status == AgentStatus.Inactive)
                    .ToListAsync();

                if (!agents.Any())
                    throw new InvalidOperationException("No inactive agents available.");

                List<TargetModel> targets = await dbContext.Targets
                    .Where(t => t.Status == TargetStatus.Live)
                    .ToListAsync();

                if (!targets.Any())
                    throw new InvalidOperationException("No live targets available.");

                KDTree<PointWithIdModel> agentsTree = GenerateDormantAgentKDTree(AgentsToPoints(agents));

                return targets.SelectMany(target =>
                {
                    Dictionary<int, double> potentialMissions = FindAgentPerTarget( agentsTree, new() { X = target.X, Y = target.Y } );
                    return potentialMissions.Select(kvp =>
                        CreateMission(target.Id, kvp.Key, kvp.Value)).ToList();
                }).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while finding possible missions.");
                return new List<MissionModel>();
            }
        }



        /*public async Task<List<MissionModel>> FindPossibleMissionsAsync()
        {
            try
            {
                List<MissionModel> missions = new();

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
        }*/

        public KDTree<PointWithIdModel> GenerateDormantAgentKDTree(List<PointWithIdModel> pointsAgents)
        {
            KDTree<PointWithIdModel> agentsTree = new KDTree<PointWithIdModel>(dimensions: 2);

            foreach (var item in pointsAgents)
            {
                agentsTree.Add(position: item.Coordinates, value: item);
            }
            return agentsTree;
        }

        public Dictionary<int, double> FindAgentPerTarget(KDTree<PointWithIdModel> agentsTree, LocationDto targetLocation)
        {
            double[] targetQuery = { targetLocation.X, targetLocation.Y };

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

        public async Task<bool> IsMissionExistAsync(int id) =>
            await dbContext.Missions.AnyAsync(m => m.Id == id);

        public async Task<MissionModel?> AllocateMissionAsync(int missionId)
        {
            try
            {
                MissionModel? mission = await dbContext.Missions.FindAsync(missionId);

                if (mission == null) { throw new Exception($"mission with id: {missionId} does not exist."); }

                AgentModel? agent = await dbContext.Agents.FindAsync(mission.AgentId);
                TargetModel? target = await dbContext.Targets.FindAsync(mission.TargetId);

                if (agent == null || target == null) return null;

                if (IsMissionLegal(agent, target))
                {
                    await ActivateMission(mission, agent);
                }

                return mission;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool IsMissionLegal(AgentModel agent, TargetModel target) =>
            IsInRange(agent, target) 
            && agent.Status == AgentStatus.Inactive 
            && target.Status == TargetStatus.Live;

        public bool IsInRange(AgentModel agent, TargetModel target) =>
            ComputeDistance(agent.X, agent.Y, target.X, target.Y) < 200
            ? true : false;

        public async Task<List<MissionModel>> UpdateAllMissionsAsync()
        {
            List<MissionModel> activeMissions =
                await dbContext.Missions
                .Where(m => m.Status == MissionStatus.Assigned)
                .ToListAsync();

            activeMissions.Where(m => !IsMissionLegal(m))
                .ToList()
                .ForEach(m => m.Status = MissionStatus.Canceled);

            List<MissionModel> updatedActiveMissions = 
                activeMissions.Where(m => m.Status == MissionStatus.Assigned)
                .ToList();

            updatedActiveMissions.ForEach(m => MoveAgentTowardsMission(m));
            return updatedActiveMissions;
        }

        public async Task<LocationModel?> MoveAgentTowardsMission(MissionModel mission)
        {
            try
            {
                LocationModel targetLocation = new() { X = mission.Target.X, Y = mission.Target.Y };
                LocationModel agentLocation = new() { X = mission.Agent.X, Y = mission.Agent.Y };

                var (x, y) = MovePointTowards(
                    mission.Target.X, mission.Target.Y, mission.Agent.X, mission.Agent.Y);

                LocationModel newLocation =  new () { X = x, Y = y };

                if (!IsLocationValid(x, y))
                {
                    throw new Exception($"Location X: {x}, Y: {y} out of range");
                }

                if (IsEliminated(targetLocation, newLocation))
                {
                    EliminationUpdate(mission);
                }

                AgentModel? agent = await dbContext.Agents.FindAsync(mission.AgentId);
                if (agent != null) { throw new Exception(); }

                agent.X = x;
                agent.Y = y;
                mission.Distance = ComputeDistance(agent.X, agent.Y, targetLocation.X, targetLocation.Y);
                mission.EstimatedDuration = ComputeTimeLeft(mission.Distance);

                await dbContext.HistoricalTimeLeft.AddAsync(
                    new EstimatedDurationsModel()
                    {
                        MissionId = mission.Id,
                        EstimatedDuration = mission.EstimatedDuration,
                    });

                await dbContext.SaveChangesAsync();
                return newLocation;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool IsMissionLegal(MissionModel mission)
        {
            AgentModel? agent = dbContext.Agents.Find(mission.AgentId);
            TargetModel? target = dbContext.Targets.Find(mission.TargetId);

            if (agent == null || target == null) { return false; }

            if (IsInRange(agent, target)
                && agent.Status == AgentStatus.Inactive
                && target.Status == TargetStatus.Live)
            { return true; }

            return false;
        }

        public async Task<bool> ActivateMission(MissionModel mission, AgentModel agent)
        {
            try
            {
                mission.Status = MissionStatus.Assigned;
                mission.StartTime = DateTime.Now;
                mission.EstimatedDuration = ComputeTimeLeft(mission.Distance);
                agent.Status = AgentStatus.Active;

                await dbContext.HistoricalTimeLeft.AddAsync(
                    new EstimatedDurationsModel()
                    {
                        MissionId = mission.Id,
                        EstimatedDuration = mission.EstimatedDuration,
                    });

                await dbContext.Missions
                    .Where(m => m.AgentId == agent.Id)
                    .ForEachAsync(m => m.Status = MissionStatus.Canceled);

                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task EliminationUpdate(MissionModel mission)
        {
            MissionModel? missionModel = await dbContext.Missions.FindAsync(mission.Id);

            if (missionModel == null) { return; }

            missionModel.Status = MissionStatus.Completed;
            missionModel.Target.Status = TargetStatus.Dead;
            missionModel.Agent.Status = AgentStatus.Inactive;
            missionModel.Agent.Eliminations++;

            await dbContext.SaveChangesAsync();
        }
    }
}

