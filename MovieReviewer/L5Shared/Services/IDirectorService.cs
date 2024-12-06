using L5Shared.Models;
using L5Shared.DTO;

namespace L5Shared.Services
{
    public interface IDirectorService
    {
        Task<ServiceResponse<List<DirectorDTO>>> GetDirectorsAsync();
        Task<ServiceResponse<DirectorDTO>> AddDirectorAsync(CreateDirectorDTO createDirectorDTO);
    }
}
