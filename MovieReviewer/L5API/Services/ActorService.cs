using L5Shared.Models;
using L5Shared.Services;
using L5Shared;
using L5Shared.DTO;
using L5API.Models;
using Microsoft.EntityFrameworkCore;

namespace L5API.Services
{
    public class ActorService : IActorService
    {
        private readonly DataContext _dataContext;

        public ActorService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<ActorDTO>> AddActorAsync(CreateActorDTO createActorDTO)
        {
            var result = new ServiceResponse<ActorDTO>();

            try
            {
                var actor = new Actor
                {
                    Name = createActorDTO.Name
                };

                await _dataContext.Actors.AddAsync(actor);
                await _dataContext.SaveChangesAsync();

                var actorDTO = new ActorDTO
                {
                    ID = actor.ID,
                    Name = actor.Name,
                };

                result.Data = actorDTO;
                result.Success = true;
                result.Message = "Added";
            }
            catch (Exception ex)
            {
                result.Message += ex.ToString();
                result.Success = false;
            }

            return result;
        }

        public async Task<ServiceResponse<List<ActorDTO>>> GetActorsAsync()
        {
            var result = new ServiceResponse<List<ActorDTO>>();

            try
            {
                var actors = await _dataContext.Actors.ToListAsync();

                var actorDTOs = actors.Select(actor => new ActorDTO
                {
                    ID = actor.ID,
                    Name = actor.Name,
                }).ToList();

                result.Data = actorDTOs;
                result.Success = true;
                result.Message = "Data received";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }

            return result;
        }
    }
}
