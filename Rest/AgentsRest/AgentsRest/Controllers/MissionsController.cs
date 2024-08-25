using AgentsRest.Dto;
using AgentsRest.Models;
using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MissionsController(
        IMissionService missionService,
        ILogger<MissionsController> logger

    ) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MissionModel>>> GetAllMissionsAsync()
        {
            try
            {
                List<MissionModel> missions = await missionService.GetAllMissionsAsync();
                return Ok(missions);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"An error occurred while fetching users. {ex.Message}"
                );
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MissionDto>> AllocateMissionAsync(int id)
        {
            try
            {
                if (id == null) { return BadRequest(); }

                if (!await missionService.IsMissionExistAsync(id)) { return NotFound($"Mission with id {id} not found."); }



                MissionModel? mission = await missionService.GetMissionByIdAsync(id);

                if (mission == null) { throw new Exception("Mission not exists."); }

                return Ok(mission);

            }
            catch (Exception ex) // Handling exceptions
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"An error occurred while fetching AllocateMission. {ex.Message}"
                );
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AgentModel>> UpdateAllMissions()
        {
            try
            {
                await missionService.UpdateAllMissionsAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"An error occurred while fetching Missions Update. {ex.Message}"
);
            }
        }
    }
}
