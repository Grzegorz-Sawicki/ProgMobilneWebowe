using L5Shared.Models;
using L5Shared.DTO;

namespace L5Shared.Services
{
    public interface IMovieService
    {
        Task<ServiceResponse<List<MovieDTO>>> GetMoviesAsync();
        Task<ServiceResponse<MovieDTO>> AddMovieAsync(CreateMovieDTO createMovieDTO);
        Task<ServiceResponse<bool>> DeleteMovieAsync(int id);
        Task<ServiceResponse<MovieDTO>> UpdateMovieAsync(UpdateMovieDTO updatedMovieDTO);
        Task<ServiceResponse<MovieDTO>> GetMovieAsync(int id);
    }
}
