namespace HelloApi.Dtos
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public int Number { get; set; }

        public int PersonId { get; set; }
        public string PersonName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public decimal GrandTotal { get; set; } 

        public List<OrderDetailReadDto> Details { get; set; } = new();
    }
}
