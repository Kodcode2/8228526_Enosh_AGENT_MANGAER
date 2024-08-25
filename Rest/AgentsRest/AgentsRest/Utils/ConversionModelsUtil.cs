// Ignore Spelling: Utils dto Util

using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Utils
{
    public class ConversionModelsUtil
    {
        public static LocationModel LocationDtoToModel(LocationDto locationDto) =>
            new LocationModel()
            {
                Id = locationDto.Id,
                X = locationDto.X,
                Y = locationDto.Y,
            };

        public static LocationDto LocationModelToDto(LocationModel locationModel) =>
            new LocationDto()
            {
                Id = locationModel.Id,
                X = locationModel.X,
                Y = locationModel.Y,
            };

        public static AgentModel AgentDtoToModel(AgentDto agentDto) =>
            new AgentModel()
            {
                Id = agentDto.Id,
                Nickname = agentDto.Nickname,
                Location = LocationDtoToModel(agentDto.Location),
                Status = agentDto.Status,
                Image = agentDto.PhotoUrl,
                Eliminations = agentDto.Eliminations,
                //AgentsMissions = agentDto.AgentsMissions, //////////
            };

        public static AgentDto AgentModelToDto(AgentModel agentModel) =>
            new AgentDto()
            {
                Id = agentModel.Id,
                Nickname = agentModel.Nickname,
                Location = LocationModelToDto(agentModel.Location),
                Status = agentModel.Status,
                PhotoUrl = agentModel.Image,
                Eliminations = agentModel.Eliminations,
                //AgentsMissions = agentModel.AgentsMissions, /////////
            };

        public static TargetModel TargetDtoToModel(TargetDto targetDto) =>
            new TargetModel()
            {
                Id = targetDto.Id,
                Name = targetDto.Name,
                Role = targetDto.Position,
                Location = LocationDtoToModel(targetDto.Location),
                Status = targetDto.Status,
                Image = targetDto.PhotoUrl,
                IsDetected = targetDto.IsDetected,
            };

        public static TargetDto TargetModelToDto(TargetModel targetModel) =>
            new TargetDto()
            {
                Id = targetModel.Id,
                Name = targetModel.Name,
                Position = targetModel.Role,
                Location = LocationModelToDto(targetModel.Location),
                Status = targetModel.Status,
                PhotoUrl = targetModel.Image,
                IsDetected = targetModel.IsDetected,
            };

        public static MissionModel MissionDtoToModel(MissionDto missionDto) =>
            new MissionModel()
            {
                Id = missionDto.Id,
                AgentId = missionDto.AgentId,
                TargetId = missionDto.TargetId,
                Distance = missionDto.Distance,
                StartTime = missionDto.StartTime,
                LeftTime = missionDto.LeftTime,
                ExecutionTime = missionDto.ExecutionTime,
                Status = missionDto.Status,
            };

        public static MissionDto MissionModelToDto(MissionModel missionModel) =>
            new MissionDto()
            {
                Id = missionModel.Id,
                AgentId = missionModel.AgentId,
                TargetId = missionModel.TargetId,
                Distance = missionModel.Distance,
                StartTime = missionModel.StartTime,
                LeftTime= missionModel.LeftTime,
                ExecutionTime = missionModel.ExecutionTime,
                Status = missionModel.Status,
            };
    }
}
