using AgentsRest.Models;
using static AgentsRest.Utils.MissionUtil;
using static AgentsRest.Utils.LocationUtil;
using Accord.Collections;
using AgentsApi.Data;
using Microsoft.EntityFrameworkCore;
using AgentsRest.Dto;
using Microsoft.Extensions.Logging;


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

        private static readonly SemaphoreSlim _semaphore = new (1, 1);

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

        public async Task<List<MissionModel>> GetAllMissionsAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                List<MissionModel> possibleMissions = await FindPossibleMissionsAsync();

                if (possibleMissions == null || !possibleMissions.Any())
                {
                    return await dbContext.Missions.ToListAsync();
                }

                List<MissionModel> existingMissions = await dbContext.Missions.ToListAsync();

                List<MissionModel> filteredMissions = possibleMissions
                    .Where(newMission =>
                        !existingMissions.Any(existMission =>
                            IsMissionsTheSame(newMission, existMission)
                        )
                    )
                    .ToList();

                if (filteredMissions.Any())
                {
                    await dbContext.Missions.AddRangeAsync(filteredMissions);
                    await dbContext.SaveChangesAsync();
                }

                return await dbContext.Missions.ToListAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<MissionModel>> FindPossibleMissionsAsync()
        {
            await _semaphore.WaitAsync();
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
                    Dictionary<int, double> potentialMissions = FindAgentPerTarget(agentsTree, new LocationDto { X = target.X, Y = target.Y });
                    return potentialMissions.Select(kvp => CreateMission(target.Id, kvp.Key, kvp.Value)).ToList();
                }).ToList();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while finding possible missions.");
                return new List<MissionModel>();
            }
            finally
            {
                _semaphore.Release();
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

        public Dictionary<int, double> FindAgentPerTarget(KDTree<PointWithIdModel> agentsTree, LocationDto targetLocation)
        {
            double[] targetQuery = { targetLocation.X, targetLocation.Y };

            double maxDistance = 200;

            var nearestAgents = agentsTree.Nearest(
                position: targetQuery,
                radius: maxDistance,
                maximum: 10 // int.MaxValue
            );

            return nearestAgents
                .OrderBy(x => x.Distance)
                .ToDictionary(
                    n => n.Node.Value.AgentId,
                    n => n.Distance
                );
        }

        public MissionModel CreateMission(int targetId, int agentId, double distance) =>
            new MissionModel()
            {
                AgentId = agentId,
                TargetId = targetId,
                Distance = distance,
                EstimatedDuration = ComputeTimeLeft(distance)
            };

        public async Task<MissionModel?> AllocateMissionAsync(int missionId)
        {
            try
            {
                MissionModel? mission = await dbContext.Missions.FindAsync(missionId);

                if (mission == null) { throw new Exception($"mission with id: {missionId} does not exist."); }

                AgentModel? agent = await dbContext.Agents.FindAsync(mission.AgentId);
                TargetModel? target = await dbContext.Targets.FindAsync(mission.TargetId);

                if (agent == null || target == null) return null;

                if (IsAllocateLegal(agent, target))
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

        public bool IsMissionsTheSame(MissionModel comparer, MissionModel second) =>
            comparer.AgentId == second.AgentId && comparer.TargetId == second.TargetId;

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

        public List<PointWithIdModel> AgentsToPoints(List<AgentModel> agents) =>
            agents.Select(AgentToPointWithId).ToList();

        public async Task<bool> IsMissionExistAsync(int id) =>
            await dbContext.Missions.AnyAsync(m => m.Id == id);

        public async Task<MissionModel?> GetMissionByIdAsync(int missionId) =>
            await dbContext.Missions.FindAsync(missionId);

        public bool IsAllocateLegal(AgentModel agent, TargetModel target) =>
            IsInRange(agent, target)
            && agent.Status == AgentStatus.Inactive
            && target.Status == TargetStatus.Live;

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

        public bool IsInRange(AgentModel agent, TargetModel target) =>
            ComputeDistance(agent.X, agent.Y, target.X, target.Y) < 200;
    }
}
