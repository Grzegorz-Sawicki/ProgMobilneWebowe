namespace ProductStorageApp.Controllers;

using ProductStorageApp.Models;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class ProductsController : Controller
{
    private readonly string productsPath;
    private readonly string productsIdPath;

    public ProductsController(IConfiguration configuration)
    {
        var dataPath = configuration["DataPath"]!;
        productsPath = Path.Combine(dataPath, "products.json");
        productsIdPath = Path.Combine(dataPath, "id_products.json");
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

    // POST /Product/Save
    [HttpPost]
    public IActionResult Save(ProductsDetailsModel model)
    {
        if (ModelState.IsValid)
        {
            var product = saveProduct(model.Product!);
            return RedirectToAction("Details", new { id = product.Id });
        }

        throw new NotImplementedException();
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

    private Product saveProduct(Product product)
    {
        if (product.Id == null)
        {
            var productsId = JsonSerializer.Deserialize<Dictionary<string, int>>(System.IO.File.ReadAllText(productsIdPath))!;
            var id = productsId["nextId"];

            product.Id = id;

            productsId["nextId"] = id + 1;
            System.IO.File.WriteAllText(productsIdPath, JsonSerializer.Serialize(productsId));
        }

        var products = JsonSerializer.Deserialize<List<Product>>(System.IO.File.ReadAllText(productsPath)) ??
            new List<Product>();
        products.RemoveAll(p => product.Id == p.Id);
        products.Add(product);

        System.IO.File.WriteAllText(productsPath,
                JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true }));

        return product;
    }
}
