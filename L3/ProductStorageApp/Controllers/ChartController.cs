using Microsoft.AspNetCore.Mvc;
using ProductStorageApp.Models;
using System.Text.Json;

namespace ProductStorageApp.Controllers
{
    public class ChartController : Controller
    {
        private string productsPath;
        private string ordersPath;
        private List<Product> products;
        private List<Order> orders;
        public ChartController(IConfiguration configuration)
        {
            var dataPath = configuration["DataPath"]!;
            productsPath = Path.Combine(dataPath, "products.json");
            ordersPath = Path.Combine(dataPath, "orders.json");

            products = getObjects<Product>(productsPath);
            orders = getObjects<Order>(ordersPath);
        }
        public IActionResult Index()
        {
            products = getObjects<Product>(productsPath);

            var viewModel = new ProductOrderModel
            {
                Products = products,
                Orders = orders
            };

            setUpOrdersByYearChart();
            setUpOrdersByMonthChart();
            setUpCategoriesQuantityChart();
            setUpCategoriesAvgPriceChart();

            return View(viewModel);
        }

        private void setUpOrdersByYearChart()
        {
            // Group orders by year and type, then count them
            var ordersGroupedByYearAndType = orders
                .GroupBy(order => new { order.Date.Year, order.Type })
                .Select(group => new
                {
                    Year = group.Key.Year,
                    Type = group.Key.Type,
                    Count = group.Count()
                })
                .ToList();

            // Prepare lists for incoming and outgoing order counts per year
            var years = ordersGroupedByYearAndType.Select(o => o.Year).Distinct().OrderBy(y => y).ToList();
            var incomingCounts = years.Select(year => ordersGroupedByYearAndType
                                            .FirstOrDefault(o => o.Year == year && o.Type == OrderType.Incoming)?.Count ?? 0)
                                            .ToList();
            var outgoingCounts = years.Select(year => ordersGroupedByYearAndType
                                            .FirstOrDefault(o => o.Year == year && o.Type == OrderType.Outgoing)?.Count ?? 0)
                                            .ToList();

            // Pass data to the view
            ViewBag.Years = years;
            ViewBag.IncomingCounts = incomingCounts;
            ViewBag.OutgoingCounts = outgoingCounts;
        }

        private void setUpOrdersByMonthChart()
        {
            // Filter orders for the year 2024 and group by month and type
            var ordersFor2024 = orders
                .Where(order => order.Date.Year == 2024)
                .GroupBy(order => new { Month = order.Date.Month, order.Type })
                .Select(group => new
                {
                    Month = group.Key.Month,
                    Type = group.Key.Type,
                    Count = group.Count()
                })
                .ToList();

            // Prepare lists for each month (1-12) for incoming and outgoing orders
            var months = Enumerable.Range(1, 12).ToList();
            var incomingCounts = months
                .Select(month => ordersFor2024
                    .FirstOrDefault(o => o.Month == month && o.Type == OrderType.Incoming)?.Count ?? 0)
                .ToList();
            var outgoingCounts = months
                .Select(month => ordersFor2024
                    .FirstOrDefault(o => o.Month == month && o.Type == OrderType.Outgoing)?.Count ?? 0)
                .ToList();

            // Pass data to the view
            ViewBag.Months = months.Select(m => new DateTime(2024, m, 1).ToString("MMMM")).ToList(); // Month names
            ViewBag.IncomingMonthCounts = incomingCounts;
            ViewBag.OutgoingMonthCounts = outgoingCounts;
        }

        private void setUpCategoriesQuantityChart()
        {
            var query = products
                .GroupBy(product => product.Category)
                .Select(group => new
                {
                    Category = group.Key,
                    CategoryNames = ((ProductCategory)group.Key).ToString(),
                    TotalQuantity = group.Sum(p => p.Quantity)
                })
                .OrderBy(categoryData => categoryData.Category)
                .ToList();

            ViewBag.Categories = query.Select(q => q.Category).ToList();
            ViewBag.CategoryNames = query.Select(q => q.CategoryNames).ToList();
            ViewBag.Quantities = query.Select(q => q.TotalQuantity).ToList();
        }

        private void setUpCategoriesAvgPriceChart()
        {
            var query = products
                .GroupBy(product => product.Category)
                .Select(group => new
                {
                    Category = group.Key,
                    CategoryNames = ((ProductCategory)group.Key).ToString(),
                    AvgPrice = group.Average(p => p.Price)
                })
                .OrderBy(categoryData => categoryData.Category)
                .ToList();

            ViewBag.AvgPrices = query.Select(q => q.AvgPrice).ToList();
        }

        private List<T> getObjects<T>(string filePath)
        {
            var jsonString = System.IO.File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(jsonString);
        }
        private void writeObjects<T>(List<T> objects, string filePath)
        {
            var jsonString = JsonSerializer.Serialize(objects, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            System.IO.File.WriteAllText(filePath, jsonString);
        }
    }
}
