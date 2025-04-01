namespace Business.DTO
{
    public class ProductImageDTO
    {
        public int? Id { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }
        [ValidateNever]
        public int? ProductId { get; set; }
        [ValidateNever]
        public int DisplayOrder { get; set; }


        [ValidateNever]
        [NotMapped]
        public ProductDTO? Product { get; set; }
    }
}
