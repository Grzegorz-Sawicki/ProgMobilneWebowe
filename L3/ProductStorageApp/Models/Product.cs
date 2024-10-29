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
        public int Id { get; set; } 
        public string Name { get; set; }
        public float Price { get; set; } 
        public ProductCategory Category { get; set; } 
        public int Quantity { get; set; }
    }
}
