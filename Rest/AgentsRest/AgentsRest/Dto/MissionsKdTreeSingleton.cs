// Ignore Spelling: Dto
using AgentsRest.Models;
using KdTree;
using KdTree.Math;

namespace AgentsRest.Dto
{
    public class MissionsKdTreeSingleton
    {
        public static readonly MissionsKdTreeSingleton Instance = new MissionsKdTreeSingleton();

        public KdTree<double, AgentModel> Tree;

        private MissionsKdTreeSingleton()
        {
            Tree = new KdTree<double, AgentModel>(2, new DoubleMath());
        }

    }
}
