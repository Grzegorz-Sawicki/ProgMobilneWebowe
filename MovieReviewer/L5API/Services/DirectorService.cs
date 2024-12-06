using L5Shared.Models;
using L5Shared.DTO;
using L5Shared.Services;
using L5Shared;
using L5API.Models;
using Microsoft.EntityFrameworkCore;

namespace L5API.Services
{
    public class DirectorService : IDirectorService
    {
        private readonly DataContext _dataContext;

        public DirectorService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<DirectorDTO>> AddDirectorAsync(CreateDirectorDTO createDirectorDTO)
        {
            var result = new ServiceResponse<DirectorDTO>();

            try
            {
                var director = new Director
                {
                    Name = createDirectorDTO.Name,
                };

                await _dataContext.Directors.AddAsync(director);
                await _dataContext.SaveChangesAsync();

                var directorDTO = new DirectorDTO
                {
                    ID = director.ID,
                    Name = director.Name,
                };

                result.Data = directorDTO;
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

        public async Task<ServiceResponse<List<DirectorDTO>>> GetDirectorsAsync()
        {
            var result = new ServiceResponse<List<DirectorDTO>>();

            try
            {
                var directors = await _dataContext.Directors.ToListAsync();

                var directorDTOs = directors.Select(director => new DirectorDTO
                {
                    ID = director.ID,
                    Name = director.Name,
                }).ToList();

                result.Data = directorDTOs;
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
