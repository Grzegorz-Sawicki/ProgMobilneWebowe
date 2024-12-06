using L5Shared.Models;
using L5Shared.DTO;
using L5Shared.Services;
using L5Shared;
using L5API.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace L5API.Services
{
    public class MovieService : IMovieService
    {
        private readonly DataContext _dataContext;

        public MovieService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<ServiceResponse<MovieDTO>> AddMovieAsync(CreateMovieDTO createMovieDTO)
        {
            var result = new ServiceResponse<MovieDTO>();

            try
            {
                var movie = new Movie
                {
                    Title = createMovieDTO.Title,
                    Details = new MovieDetails
                    {
                        ReleaseDate = createMovieDTO.ReleaseDate,
                        Length = createMovieDTO.Length,
                    },
                    Note = new MovieNote
                    {
                        Rating = createMovieDTO.Rating,
                        Review = createMovieDTO.Review,
                    },
                    DirectorID = createMovieDTO.DirectorID,
                    Director = await _dataContext.Directors
                        .FirstOrDefaultAsync(d => d.ID == createMovieDTO.DirectorID),
                    Actors = await _dataContext.Actors
                        .Where(actor => createMovieDTO.ActorIDs.Contains(actor.ID))
                        .ToListAsync()
                };

                await _dataContext.Movies.AddAsync(movie);
                await _dataContext.SaveChangesAsync();

                var movieDTO = new MovieDTO
                {
                    ID = movie.ID,
                    Title = movie.Title,
                    ReleaseDate = movie.Details.ReleaseDate,
                    Length = movie.Details.Length,
                    Rating = movie.Note.Rating,
                    Review = movie.Note.Review,
                    DirectorID = movie.DirectorID,
                    DirectorName = movie.Director.Name,
                    Actors = movie.Actors.Select(actor => new ActorDTO
                    {
                        ID = actor.ID,
                        Name = actor.Name
                    }).ToList()
                };

                result.Data = movieDTO;
                result.Success = true;
                result.Message = "Data created";
            }
            catch (Exception ex)
            {
                result.Message = ex.InnerException?.Message ?? ex.Message;
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResponse<bool>> DeleteMovieAsync(int id)
        {
            var result = new ServiceResponse<bool>();

            try
            {
                var movie = new Movie() { ID = id };
                _dataContext.Movies.Attach(movie);
                _dataContext.Movies.Remove(movie);
                await _dataContext.SaveChangesAsync();

                result.Data = true;
                result.Success = true;
                result.Message = "Data deleted";
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.Message = ex.Message;
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResponse<MovieDTO>> GetMovieAsync(int id)
        {
            var result = new ServiceResponse<MovieDTO>();

            try
            {
                var movie = await _dataContext.Movies
                    .Include(m => m.Director)
                    .Include(m => m.Actors)
                    .Include(m => m.Details)
                    .Include(m => m.Note)
                    .FirstAsync(m => m.ID == id);

                var movieDTO = new MovieDTO
                {
                    ID = movie.ID,
                    Title = movie.Title,
                    ReleaseDate = movie.Details.ReleaseDate,
                    Length = movie.Details.Length,
                    Rating = movie.Note.Rating,
                    Review = movie.Note.Review,
                    DirectorID = movie.DirectorID,
                    DirectorName = movie.Director.Name,
                    Actors = movie.Actors.Select(actor => new ActorDTO
                    {
                        ID = actor.ID,
                        Name = actor.Name
                    }).ToList()
                };

                result.Data = movieDTO;
                result.Success = true;
                result.Message = "Data retrieved successfully";
            }
            catch (Exception ex)
            {
                result.Message = $"Error retrieving data from the database {ex.Message}";
                result.Success = false;
            }
            return result;
        }

        public async Task<ServiceResponse<List<MovieDTO>>> GetMoviesAsync()
        {
            var result = new ServiceResponse<List<MovieDTO>>();

            try
            {
                var movies = await _dataContext.Movies
                    .Include(m => m.Director)
                    .Include(m => m.Actors)
                    .Include(m => m.Details)
                    .Include(m => m.Note)
                    .ToListAsync();

                var movieDTOs = movies.Select(movie => new MovieDTO
                {
                    ID = movie.ID,
                    Title = movie.Title,
                    ReleaseDate = movie.Details.ReleaseDate,
                    Length = movie.Details.Length,
                    Rating = movie.Note.Rating,
                    Review = movie.Note.Review,
                    DirectorID = movie.DirectorID,
                    DirectorName = movie.Director.Name,
                    Actors = movie.Actors.Select(actor => new ActorDTO
                    {
                        ID = actor.ID,
                        Name = actor.Name
                    }).ToList()
                }).ToList();

                result.Data = movieDTOs;
                result.Success = true;
                result.Message = "Data retrieved";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Success = false;
            }

            return result;
        }

        public async Task<ServiceResponse<MovieDTO>> UpdateMovieAsync(UpdateMovieDTO updatedMovieDTO)
        {
            var result = new ServiceResponse<MovieDTO>();

            try
            {
                // Retrieve the movie by ID
                var movie = await _dataContext.Movies
                    .Include(m => m.Director)
                    .Include(m => m.Actors)
                    .Include(m => m.Details)
                    .Include(m => m.Note)
                    .FirstOrDefaultAsync(m => m.ID == updatedMovieDTO.ID);

                if (movie == null)
                {
                    result.Success = false;
                    result.Message = "Movie not found.";
                    return result;
                }

                // Update the Movie properties
                movie.Title = updatedMovieDTO.Title;
                movie.Details.ReleaseDate = updatedMovieDTO.ReleaseDate;
                movie.Details.Length = updatedMovieDTO.Length;
                movie.Note.Rating = updatedMovieDTO.Rating;
                movie.Note.Review = updatedMovieDTO.Review;

                // Update Director if the DirectorID is different
                if (movie.DirectorID != updatedMovieDTO.DirectorID)
                {
                    var director = await _dataContext.Directors
                        .FirstOrDefaultAsync(d => d.ID == updatedMovieDTO.DirectorID);

                    if (director != null)
                    {
                        movie.DirectorID = updatedMovieDTO.DirectorID;
                        movie.Director = director; // Update Director association
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "Director not found.";
                        return result;
                    }
                }

                // Update Actors if ActorIDs have changed
                if (updatedMovieDTO.ActorIDs != null)
                {
                    movie.Actors = await _dataContext.Actors
                        .Where(actor => updatedMovieDTO.ActorIDs.Contains(actor.ID))
                        .ToListAsync();
                }

                // Save changes to the database
                await _dataContext.SaveChangesAsync();

                // Map the updated Movie to MovieDTO
                var movieDTO = new MovieDTO
                {
                    ID = movie.ID,
                    Title = movie.Title,
                    ReleaseDate = movie.Details.ReleaseDate,
                    Length = movie.Details.Length,
                    Rating = movie.Note.Rating,
                    Review = movie.Note.Review,
                    DirectorID = movie.DirectorID,
                    DirectorName = movie.Director.Name,
                    Actors = movie.Actors.Select(actor => new ActorDTO
                    {
                        ID = actor.ID,
                        Name = actor.Name
                    }).ToList()
                };

                result.Data = movieDTO;
                result.Success = true;
                result.Message = "Movie updated successfully.";
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
