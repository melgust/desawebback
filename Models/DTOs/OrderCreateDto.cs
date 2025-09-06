namespace HelloApi.Dtos
{
    public class OrderCreateDto
    {
        public int PersonId { get; set; }
        public List<OrderDetailCreateDto> Details { get; set; } = new();
        public int? CreatedBy { get; set; }
    }
}
