namespace ProductStorageApp.Models;

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.ComponentModel.DataAnnotations;

public class ProductsDetailsModel
{
    [Required]
    public Product? Product { get; set; }

    public bool IsCreateProduct { get; set; } = false;

    public IEnumerable<SelectListItem> ProductCategories { get; set; } =
        Enum.GetValues<ProductCategory>()
            .Select(category => new SelectListItem(category.ToString(), category.ToString()));

    public ProductsDetailsModel() { }

    public ProductsDetailsModel(Product? product)
    {
        Product = product;
    }
}
