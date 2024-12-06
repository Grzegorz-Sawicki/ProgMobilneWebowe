using L5Shared.Models;
using L5Shared.DTO;

namespace L5Shared.Services
{
    public interface IActorService
    {
        Task<ServiceResponse<List<ActorDTO>>> GetActorsAsync();
        Task<ServiceResponse<ActorDTO>> AddActorAsync(CreateActorDTO createActorDTO);
    }
}
