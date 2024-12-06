using L5Shared.DTO;

namespace L5Mvc.Models;

public class MoviesViewModel
{
    public List<MovieViewModel> Movies { get; set; } = new();
}
