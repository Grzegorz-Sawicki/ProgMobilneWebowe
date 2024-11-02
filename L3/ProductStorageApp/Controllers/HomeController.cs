using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductStorageApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ProductStorageApp.Controllers
{
    public class HomeController : Controller
    {
        private string productsPath;
        private string ordersPath;
        private string productsIdPath;

        private List<Product> products;

        public HomeController(IConfiguration configuration)
        {
            var dataPath = configuration["DataPath"]!;
            productsPath = Path.Combine(dataPath, "products.json");
            ordersPath = Path.Combine(dataPath, "orders.json");
            productsIdPath = Path.Combine(dataPath, "id_products.json");

            products = getProducts();
        }

        private List<Product> getProducts()
        {
            var jsonStringProducts = System.IO.File.ReadAllText(productsPath);
            return JsonSerializer.Deserialize<List<Product>>(jsonStringProducts);
        }
        private void writeObjects<T>(List<T> objects, string filePath)
        {
            var jsonString = JsonSerializer.Serialize(objects, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            System.IO.File.WriteAllText(filePath, jsonString);
        }
        public IActionResult Index()
        {
            //Products
            products = getProducts();

            var viewModel = new ProductOrderModel
            {
                ProductCategories = new SelectList(Enum.GetValues(typeof(ProductCategory))),
                Products = products,
                OrderTypes = new SelectList(Enum.GetValues(typeof(OrderType)))
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            var jsonString = System.IO.File.ReadAllText(productsPath);

            var objects = JsonSerializer.Deserialize<List<Product>>(jsonString) ?? new List<Product>();

            var idCounterData = JsonSerializer.Deserialize<Dictionary<string, int>>(System.IO.File.ReadAllText(productsIdPath));
            int nextId = idCounterData["nextId"];
            product.Id = nextId;
            objects.Add(product);

            writeObjects(objects, productsPath);

            idCounterData["nextId"] = nextId + 1;
            System.IO.File.WriteAllText(productsIdPath, JsonSerializer.Serialize(idCounterData));

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CreateOrder(Order order)
        {
            //modyfikacja ilości danego produktu
            int productIdToUpdate = order.ProductId;
            Product productToUpdate = products.FirstOrDefault(p => p.Id == order.ProductId);
            if (productToUpdate != null)
            {
                if (order.Type == OrderType.Incoming)
                {
                    productToUpdate.Quantity += order.ProductQuantity;
                }
                else if (order.Type == OrderType.Outgoing && productToUpdate.Quantity >= order.ProductQuantity)
                {
                    productToUpdate.Quantity -= order.ProductQuantity;
                }
                else //zamówiono więcej produktów niż aktualnie jest
                {
                    return RedirectToAction("Index");
                }
            }

            var jsonString = System.IO.File.ReadAllText(ordersPath);
            var objects = JsonSerializer.Deserialize<List<Order>>(jsonString) ?? new List<Order>();
            order.Id = objects.Any() ? objects.Max(o => o.Id) + 1 : 1;
            objects.Add(order);

            writeObjects(products, productsPath);

            writeObjects(objects, ordersPath);

            return RedirectToAction("Index");
        }

    }
}
