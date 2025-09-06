namespace HelloApi.Dtos
{
    public class ItemUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
