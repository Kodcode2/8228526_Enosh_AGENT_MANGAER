using AgentsRest.Dto;
using AgentsRest.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace AgentsRest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController(IJwtService jwtService) : ControllerBase
    {
        private static readonly ImmutableList<string> allowedNames = [
            "SimulationServer", "MVCServer"
        ];

        [HttpPost]
        public ActionResult<string> Login([FromBody] LoginDto loginDto) =>
            allowedNames.Contains(loginDto.Name)
                ? Ok(jwtService.CreateToken(loginDto.Name))
                : BadRequest();

        [Authorize]
        [HttpGet("protected")]
        public ActionResult<string> Protected()
        {
            return Ok("Ok!");
        }
    }
}
