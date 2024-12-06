using L5Shared.DTO;
using L5Shared.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace L5Mvc.Models;

public class MovieViewModel
{
    public int? Id { get; set; }

    [Required]
    public string? Title { get; set; }

    [Range(0, 5)]
    public float Rating { get; set; } = 5.0f;

    [Required]
    public string? Review { get; set; }

    public DateTime ReleaseDate { get; set; } = DateTime.Now;

    public int Length { get; set; } = 120;

    public DirectorViewModel? Director { get; set; }

    [Required]
    public int? DirectorId { get; set; }

    public List<int> ActorIds { get; set; } = new();
    public List<ActorViewModel> Actors = new();

    public SelectList? AllDirectors { get; set; }
    public MultiSelectList? AllActors { get; set; }
}
