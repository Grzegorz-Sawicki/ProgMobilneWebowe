using System.ComponentModel.DataAnnotations;

namespace ProductStorageApp.Models
{
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        HomeAppliances,
        Books,
        HealthAndBeauty,
        SportsAndOutdoors,
        ToysAndGames,
        FoodAndBeverage,
        Automotive,
        OfficeSupplies
    }

    public class Product
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public ProductCategory Category { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
