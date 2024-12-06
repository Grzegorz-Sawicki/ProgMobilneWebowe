using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using L5Mvc.Models;
using L5Shared.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using L5Shared.DTO;
using L5Shared.Models;

namespace L5Mvc.Controllers;

public class DirectorsController : Controller
{
    private readonly ILogger<DirectorsController> _logger;

    private readonly IDirectorService _directorService;

    public DirectorsController(ILogger<DirectorsController> logger, IDirectorService directorService)
    {
        _logger = logger;
        _directorService = directorService;
    }

    public async Task<IActionResult> IndexAsync()
    {
        DirectorsViewModel model = new();

        var directorDtos = (await _directorService.GetDirectorsAsync())?.Data!;
        model.Directors = directorDtos.Select(director => new DirectorViewModel { Id = director.ID, Name = director.Name }).ToList();

        return View(model);
    }

    [HttpPost]
    [ActionName("Index")]
    public async Task<IActionResult> IndexPostAsync(DirectorViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Index");
        }

        var createDirectorDTO = new CreateDirectorDTO
        {
            Name = model.Name
        };

        await _directorService.AddDirectorAsync(createDirectorDTO);
        return RedirectToAction("Index");
    }
}
