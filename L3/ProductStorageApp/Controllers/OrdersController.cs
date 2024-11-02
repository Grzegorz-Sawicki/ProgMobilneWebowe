using Microsoft.AspNetCore.Mvc;
using ProductStorageApp.Utils;
using ProductStorageApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace ProductStorageApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly string ordersPath;
        private readonly string productsPath;

        private List<Product> products;
        private List<Order> orders;

        public OrdersController(IConfiguration configuration)
        {
            var dataPath = configuration["DataPath"];
            ordersPath = Path.Combine(dataPath, "orders.json");
            productsPath = Path.Combine(dataPath, "products.json");

            products = Utils.Utils.getObjects<Product>(productsPath);
            orders = Utils.Utils.getObjects<Order>(ordersPath);
        }
        public IActionResult Index()
        {
            orders = Utils.Utils.getObjects<Order>(ordersPath);
            orders.Sort((a, b) => a.Date.CompareTo(b.Date));
            orders.Reverse();

            products = Utils.Utils.getObjects<Product>(productsPath);

            var orderTypes = new SelectList(Enum.GetValues(typeof(OrderType)));

            return View(new OrdersModel(orders, products, orderTypes));
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

            Utils.Utils.writeObjects(products, productsPath);

            Utils.Utils.writeObjects(objects, ordersPath);

            return RedirectToAction("Index");
        }

    }
}
