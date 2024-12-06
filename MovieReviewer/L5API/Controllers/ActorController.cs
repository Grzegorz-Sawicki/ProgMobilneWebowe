using L5Shared.Models;
using L5Shared.DTO;
using L5Shared;
using L5Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace L5API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : Controller
    {
        private readonly IActorService _actorService;
        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Actor>>>> GetActors()
        {
            var result = await _actorService.GetActorsAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(500, result.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<ActorDTO>>> AddActor([FromBody] CreateActorDTO createActorDTO)
        {
            var result = await _actorService.AddActorAsync(createActorDTO);

            if(result.Success) 
                return Ok(result);
            else 
                return StatusCode(500, result.Message);
        }
    }
}
