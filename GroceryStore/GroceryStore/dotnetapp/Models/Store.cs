namespace dotnetapp.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public int StockQuantity { get; set; }
        public int Price { get; set; }
        public string ExpiryDate { get; set; }
    }
}
