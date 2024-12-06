using L5Shared.DTO;

namespace L5Mvc.Models;

public class DirectorsViewModel
{
    public List<DirectorViewModel> Directors { get; set; } = new();

    public string? Name { get; set; }
}
