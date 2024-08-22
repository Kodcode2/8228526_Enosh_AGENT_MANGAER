﻿// Ignore Spelling: Dto

using AgentsRest.Models;
using System.ComponentModel.DataAnnotations;

namespace AgentsRest.Dto
{
    public class MissionDto
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public int TargetId { get; set; }
        public DateTime TimeLeft { get; set; }
        public DateTime? ExecutionTime { get; set; }
        public MissionStatus Status { get; set; } = MissionStatus.Proposal;
        public AgentDto? Agent { get; set; }
        public TargetDto? Target { get; set; }
    }
}
