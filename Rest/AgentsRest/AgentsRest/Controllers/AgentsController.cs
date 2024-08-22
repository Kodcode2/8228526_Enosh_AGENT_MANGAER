// Ignore Spelling: Dto

using AgentsApi.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using AgentsRest.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AgentsController(
        ApplicationDbContext dbContext, 
        IAgentService agentService,
        ILogger<AgentsController> logger

        ) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AgentModel>>> GetAllAgents()
        {
            try
            {
                List<AgentModel> agents = await agentService.GetAllAgentsAsync();
                return Ok(agents);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"An error occurred while fetching users. {ex.Message}"
                );
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AgentModel>> GetAgentByIdAsync(int id)
        {
            try
            {
                AgentModel? agent = await agentService.GetAgentByIdAsync(id); // Try find the agent
                if (agent == null) { return NotFound($"Agent with id {id} does not exist."); } // return 404 if not exist
                return Ok(agent); // return 200 with the agent
            }
            catch (Exception ex) // Handling exceptions
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"An error occurred while fetching users. {ex.Message}"
                );
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AgentModel>> CreateAgentAsync([FromBody] AgentDto agentDto)
        {
            try
            {
                AgentModel? target = await agentService.CreateAgentAsync(agentDto);
                return Ok(target);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/pin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LocationDto>> PinAgentAsync(int id, [FromBody] LocationDto locationDto) // does not work  yet !!!
        {
            try
            {
                if (id == null || locationDto == null) { return BadRequest(); }
            
                if (! await agentService.IsAgentExistAsync(id)) { return NotFound($"Agent with id {id} not found."); }

                AgentModel? agent = await agentService.PlaceAgentAsync(id, locationDto);

                return Ok(agent);
                
            }
            catch (Exception ex) // Handling exceptions
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"An error occurred while fetching users. {ex.Message}"
                );
            }
        }
    }
}
