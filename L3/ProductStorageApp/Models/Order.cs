namespace ProductStorageApp.Models
{
    public enum OrderType
    {
        Incoming,
        Outgoing
    }
    public class Order
    {
        public int Id { get; set; }
        public OrderType Type { get; set; }
        public DateOnly Date { get; set; }
        public int ProductId {  get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public int ProductQuantity { get; set; }
    }
}
