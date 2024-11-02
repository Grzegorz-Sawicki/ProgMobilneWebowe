using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProductStorageApp.Models
{
    public class ProductOrderModel
    {
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
    }
}
