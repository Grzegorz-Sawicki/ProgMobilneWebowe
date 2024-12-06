using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using L5Mvc.Models;
using L5Shared.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using L5Shared.DTO;
using L5Shared.Models;

namespace L5Mvc.Controllers;

public class ActorsController : Controller
{
    private readonly ILogger<ActorsController> _logger;

    private readonly IActorService _actorService;

    public ActorsController(ILogger<ActorsController> logger, IActorService actorService)
    {
        _logger = logger;
        _actorService = actorService;
    }

    public async Task<IActionResult> IndexAsync()
    {
        ActorsViewModel model = new();

        var actorDtos = (await _actorService.GetActorsAsync())?.Data!;
        model.Actors = actorDtos.Select(actor => new ActorViewModel { Id = actor.ID, Name = actor.Name }).ToList();

        return View(model);
    }

    [HttpPost]
    [ActionName("Index")]
    public async Task<IActionResult> IndexPostAsync(ActorViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

        var createActorDTO = new CreateActorDTO
        {
            Name = model.Name
        };

        await _actorService.AddActorAsync(createActorDTO);
        return RedirectToAction("Index");
    }
}
