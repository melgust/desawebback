namespace HelloApi.Dtos
{
    public class ItemCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? CreatedBy { get; set; }
    }
}
