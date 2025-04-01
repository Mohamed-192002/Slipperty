namespace Business.DTO
{
    public class ProductImageOrderDTO
    {
        public int? Id { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }
        [ValidateNever]
        public int DisplayOrder { get; set; }
        [ValidateNever]
        public bool isSaved { get; set; } = true;
    }
}
