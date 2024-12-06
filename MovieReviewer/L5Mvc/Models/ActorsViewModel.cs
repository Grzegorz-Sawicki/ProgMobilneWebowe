using L5Shared.DTO;

namespace L5Mvc.Models;

public class ActorsViewModel
{
    public List<ActorViewModel> Actors { get; set; } = new();

    public string? Name { get; set; }
}
