namespace ProductStorageApp.Controllers;

using ProductStorageApp.Models;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class ProductsController : Controller
{
    private readonly string productsPath;

    public ProductsController(IConfiguration configuration)
    {
        var dataPath = configuration["DataPath"]!;
        productsPath = Path.Combine(dataPath, "products.json");
    }

    // GET /Products
    public IActionResult Index(int? id)
    {
        var products = getProducts();
        return View(new ProductsModel(products));
    }

    // GET /Products/Details/{id}
    public IActionResult Details(int id)
    {
        var product = getProductById(id);
        return View(new ProductsDetailsModel(product));
    }

    private Product? getProductById(int id)
    {
        var products = getProducts();
        return products.Find(product => product.Id == id);
    }

    private List<Product> getProducts()
    {
        var products = JsonSerializer.Deserialize<List<Product>>(System.IO.File.ReadAllText(productsPath))!;
        products.Sort((a, b) => a.Name.CompareTo(b.Name));
        return products;
    }
}
