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
    public IActionResult Index(string? name, string? category)
    {
        var products = getProducts();

        if (name != null)
        {
            products = products.Where(product => product.Name.ToLower()
                        .StartsWith(name.ToLower()));
        }

        if (category != null)
        {
            products = products.Where(product => product.Category.ToString() == category);
        }

        var model = new ProductsModel(products);
        model.SearchName = name;
        return View(model);
    }

    // GET /Products/Details/{id?}
    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            var model = new ProductsDetailsModel(new Product());
            model.IsCreateProduct = true;
            return View(model);
        }

        var product = getProductById((int)id!);
        return View(new ProductsDetailsModel(product));
    }

    // POST /Product/Save
    [HttpPost]
    public IActionResult Save(ProductsDetailsModel model)
    {
        if (ModelState.IsValid)
        {
            var previous = model.Product?.Id != null ? getProductById((int)model.Product.Id) : null;
            var saved = model.Product!;
            if (previous != null)
            {
                saved.Quantity = previous.Quantity;
            }

            saved = saveProduct(saved);
            return RedirectToAction("Details", new { id = saved.Id });
        }

        throw new NotImplementedException();
    }

    // POST /Product/Remove
    [HttpPost]
    public IActionResult Remove(ProductsDetailsModel model)
    {
        if (ModelState.IsValid)
        {
            removeProduct(model.Product!);
            return RedirectToAction("Index");
        }

        throw new NotImplementedException();
    }

    private Product? getProductById(int id)
    {
        var products = getProducts();
        return products.FirstOrDefault(product => product.Id == id);
    }

    private IEnumerable<Product> getProducts()
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

            int id;
            if (!productsId.TryGetValue("nextId", out id))
            {
                id = 1;
            }

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

    private void removeProduct(Product product)
    {
        var products = JsonSerializer.Deserialize<List<Product>>(System.IO.File.ReadAllText(productsPath)) ??
            new List<Product>();
        products.RemoveAll(p => product.Id == p.Id);

        System.IO.File.WriteAllText(productsPath,
                JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true }));
    }
}
