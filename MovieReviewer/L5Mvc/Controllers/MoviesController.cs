using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using L5Mvc.Models;
using L5Shared.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using L5Shared.DTO;
using L5Shared.Models;

namespace L5Mvc.Controllers;

public class MoviesController : Controller
{
    private readonly ILogger<MoviesController> _logger;

    private readonly IMovieService _movieService;
    private readonly IDirectorService _directorService;
    private readonly IActorService _actorService;

    public MoviesController(ILogger<MoviesController> logger, IMovieService movieService, IDirectorService directorService, IActorService actorService)
    {
        _logger = logger;
        _movieService = movieService;
        _directorService = directorService;
        _actorService = actorService;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var moviesDto = (await _movieService.GetMoviesAsync())?.Data!;

        MoviesViewModel model = new();

        List<MovieViewModel> movies = new(moviesDto.Select(movie => new MovieViewModel
        {
            Id = movie.ID,
            Title = movie.Title,
            ReleaseDate = movie.ReleaseDate,
            Length = movie.Length,
            Rating = movie.Rating,
            Review = movie.Review,
            Director = new DirectorViewModel { Id = movie.DirectorID, Name = movie.DirectorName },
            Actors = new List<ActorViewModel>(movie.Actors.Select(actor => new ActorViewModel { Id = actor.ID, Name = actor.Name }))
        }));

        model.Movies = movies;

        return View(model);
    }

    public async Task<IActionResult> DetailsAsync(int? id)
    {
        MovieViewModel model = new();

        var directorDtos = (await _directorService.GetDirectorsAsync())?.Data!;
        List<DirectorViewModel> directors = new(directorDtos.Select(director => new DirectorViewModel
        {
            Id = director.ID,
            Name = director.Name,
        }));

        model.AllDirectors = new SelectList(directors, nameof(DirectorViewModel.Id), nameof(DirectorViewModel.Name), directors[0]);

        var actorDtos = (await _actorService.GetActorsAsync())?.Data!;
        List<ActorViewModel> actors = new(actorDtos.Select(actor => new ActorViewModel
        {
            Id = actor.ID,
            Name = actor.Name,
        }));

        model.AllActors = new MultiSelectList(actors, nameof(ActorViewModel.Id), nameof(ActorViewModel.Name));

        if (id == null)
        {
            return View(model);
        }

        var movieDto = (await _movieService.GetMovieAsync((int)id))?.Data!;

        model.Id = movieDto.ID;
        model.Title = movieDto.Title;
        model.ReleaseDate = movieDto.ReleaseDate;
        model.Length = movieDto.Length;
        model.Rating = movieDto.Rating;
        model.Review = movieDto.Review;
        model.DirectorId = movieDto.DirectorID;
        model.ActorIds = movieDto.Actors.Select(actor => actor.ID).ToList();
        model.AllActors = new MultiSelectList(
            actors,
            nameof(ActorViewModel.Id),
            nameof(ActorViewModel.Name),
            movieDto.Actors.Select(actor => actor.ID)
        );

        return View(model);
    }

    [HttpPost]
    [ActionName("Details")]
    public async Task<IActionResult> DetailsPostAsync(MovieViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Details");
        }

        MovieDTO movieDto;
        if (model.Id == null)
        {
            var createMovieDto = new CreateMovieDTO
            {
                Title = model.Title,
                DirectorID = (int)model?.DirectorId,
                Length = model.Length,
                ReleaseDate = model.ReleaseDate,
                Rating = model.Rating,
                Review = model.Review,
                ActorIDs = model.ActorIds
            };
            var response = await _movieService.AddMovieAsync(createMovieDto);

            movieDto = response?.Data!;
        }
        else
        {
            var updateMovieDto = new UpdateMovieDTO
            {
                ID = (int)model?.Id,
                Title = model.Title,
                DirectorID = (int)model?.DirectorId,
                Length = model.Length,
                ReleaseDate = model.ReleaseDate,
                Rating = model.Rating,
                Review = model.Review,
                ActorIDs = model.ActorIds
            };
            movieDto = (await _movieService.UpdateMovieAsync(updateMovieDto))?.Data!;
        }

        var id = movieDto.ID;
        return RedirectToAction("Details", new { id });
    }

    [ActionName("Delete")]
    public async Task<IActionResult> DetailsDeleteAsync(int id)
    {
        await _movieService.DeleteMovieAsync(id);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
