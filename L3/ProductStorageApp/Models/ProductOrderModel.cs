using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProductStorageApp.Models
{
    public class ProductOrderModel
    {
        public Product Product { get; set; }
        public Order Order { get; set; }

        public ProductCategory ProductCategory { get; set; }
        public SelectList ProductCategories { get; set; }

        public List<Product> Products { get; set; }

        public OrderType OrderType { get; set; }
        public SelectList OrderTypes { get; set; }
    }
}
