namespace ProductStorageApp.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.ComponentModel.DataAnnotations;

    public class OrdersModel
    {
        [Required]
        public List<Order>? Orders { get; set; }
        [Required]
        public List<Product> Products { get; set; }
        [Required]
        public SelectList OrderTypes { get; set; }

        public OrdersModel(List<Order>? orders, List<Product> products, SelectList orderTypes)
        {
            Orders = orders;
            Products = products;
            OrderTypes = orderTypes;
        }
    }
}
