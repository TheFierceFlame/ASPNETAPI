namespace NotificationsAPI.DTO
{
    public class ProductSaleDTO
    {
        public string Category { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public decimal UnitaryPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime Date { get; set; }
    }
}
