namespace Business.DTO
{
    public class RelatedProductDTO
    {
        public int? Id { get; set; }

        [Display(Name = "المنتج")]
        [Required(ErrorMessage = "اختر المنتج")]
        public int? RelatedProductId { get; set; }

        public int? ProductId { get; set; }


        [ValidateNever]
        [NotMapped]
        public ProductDTO? Product { get; set; }
        
        [ValidateNever]
        [NotMapped]
        public ProductDTO? RProduct { get; set; }
    }
}
