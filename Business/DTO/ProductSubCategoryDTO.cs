namespace Business.DTO
{
    public class ProductSubCategoryDTO
    {
        public int? Id { get; set; }

        [ValidateNever]
        public int? SubCategoryId { get; set; }

        [ValidateNever]
        public int? ProductId { get; set; }


        [ValidateNever]
        [NotMapped]
        public ProductDTO? Product { get; set; }
        [ValidateNever]
        [NotMapped]
        public SubCategoryDTO? SubCategory { get; set; }
    }
}
