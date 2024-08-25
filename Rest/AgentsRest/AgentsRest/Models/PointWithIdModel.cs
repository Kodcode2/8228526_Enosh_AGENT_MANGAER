namespace AgentsRest.Models
{
    public class PointWithIdModel
    {
        public double[] Coordinates { get; set; }
        public int AgentId { get; set; }

/*        public PointWithIdModel(double[] coordinates, int agentId)
        {
            Coordinates = coordinates;
            AgentId = agentId;
        }*/
    }
}
