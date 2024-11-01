namespace ProductStorageApp.Controllers;

using ProductStorageApp.Models;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class ProductsController : Controller
{
    private readonly string productsPath;

    public ProductsController(IWebHostEnvironment webHostEnvironment)
    {
        productsPath = Path.Combine(webHostEnvironment.WebRootPath, "json/products.json");
    }

    // GET /Products
    public IActionResult Index(int? id)
    {
        var products = getProducts();
        return View(new ProductsModel(products));
    }

    private List<Product> getProducts()
    {
        var jsonStringProducts = System.IO.File.ReadAllText(productsPath);
        return JsonSerializer.Deserialize<List<Product>>(jsonStringProducts)!;
    }
}
