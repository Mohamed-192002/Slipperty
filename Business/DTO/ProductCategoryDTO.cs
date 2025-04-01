namespace Business.DTO
{
    public class ProductCategoryDTO
    {
        public int? Id { get; set; }

        [ValidateNever]
        public int? CategoryId { get; set; }

        [ValidateNever]
        public int? ProductId { get; set; }


        [ValidateNever]
        [NotMapped]
        public ProductDTO? Product { get; set; }
        [ValidateNever]
        [NotMapped]
        public CategoryDTO? Category { get; set; }
    }
}
