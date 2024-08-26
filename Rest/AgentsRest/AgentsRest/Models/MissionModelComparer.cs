// Ignore Spelling: comparer

namespace AgentsRest.Models
{
    public class MissionModelComparer : IEqualityComparer<MissionModel>
    {
        public bool Equals(MissionModel exist, MissionModel comparer) =>
            exist.AgentId == comparer.AgentId && exist.TargetId == comparer.TargetId;

        public int GetHashCode(MissionModel obj) =>
            HashCode.Combine(obj.AgentId, obj.TargetId);
    }
}
