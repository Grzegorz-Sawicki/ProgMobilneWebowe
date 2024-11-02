namespace ProductStorageApp.Models;

using System.ComponentModel.DataAnnotations;

public class ProductsModel
{
    [Required]
    public List<Product>? Products { get; set; }

    public ProductsModel(List<Product>? products)
    {
        Products = products;
    }
}
