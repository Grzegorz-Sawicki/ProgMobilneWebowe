using L5Shared.Models;
using L5Shared.DTO;
using L5Shared;
using L5Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace L5API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : Controller
    {
        private readonly IDirectorService _directorService;
        public DirectorController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Director>>>> GetDirectors()
        {
            var result = await _directorService.GetDirectorsAsync();
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
        public async Task<ActionResult<ServiceResponse<DirectorDTO>>> AddDirector([FromBody] CreateDirectorDTO createDirectorDTO)
        {
            var result = await _directorService.AddDirectorAsync(createDirectorDTO);

            if(result.Success) 
                return Ok(result);
            else 
                return StatusCode(500, result.Message);
        }
    }
}
