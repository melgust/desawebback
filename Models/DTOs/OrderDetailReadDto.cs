namespace HelloApi.Dtos
{
    public class OrderDetailReadDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
