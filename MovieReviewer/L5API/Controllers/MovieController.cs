using L5Shared.Models;
using L5Shared.Services;
using L5Shared.DTO;
using L5Shared;
using Microsoft.AspNetCore.Mvc;
using L5API.Services;

namespace L5API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<MovieDTO>>>> GetMovies()
        {
            var result = await _movieService.GetMoviesAsync();

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(500, $"no {result.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<MovieDTO>>> GetMovie([FromRoute] int id)
        {
            var result = await _movieService.GetMovieAsync(id);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, result.Message);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<MovieDTO>>> AddMovie([FromBody] CreateMovieDTO createMovieDTO)
        {
            var result = await _movieService.AddMovieAsync(createMovieDTO);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteMovie([FromRoute] int id)
        {
            var result = await _movieService.DeleteMovieAsync(id);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, result.Message);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<MovieDTO>>> UpdateMovie([FromBody] UpdateMovieDTO updatedMovie)
        {
            var result = await _movieService.UpdateMovieAsync(updatedMovie);

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, result.Message);
        }
    }
}
