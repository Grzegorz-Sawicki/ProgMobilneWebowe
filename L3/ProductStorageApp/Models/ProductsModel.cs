namespace ProductStorageApp.Models;

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class ProductsModel
{
    [Required]
    public IEnumerable<Product>? Products { get; set; }

    public string SearchName { get; set; }

    public IEnumerable<SelectListItem> ProductCategories { get; set; } =
        Enum.GetValues<ProductCategory>()
            .Select(category => new SelectListItem(category.ToString(), category.ToString()));

    public ProductsModel(IEnumerable<Product>? products)
    {
        Products = products;
    }
}
