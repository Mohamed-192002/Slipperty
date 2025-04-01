namespace Business.DTO
{
    public class ProductCountsOfferDTO
    {
        public int? Id { get; set; }
        [Display(Name = "عدد القطع")]
        [Required(ErrorMessage = "ادخل عدد القطع")]
        public int Count { get; set; }

        [Display(Name = "السعر")]
        [Required(ErrorMessage = "ادخل السعر")]
        [Precision(18,2)]
        public decimal Price { get; set; }
        
        [Display(Name = "الخصم")]
        [Required(ErrorMessage = "الخصم")]
        [Precision(18,2)]
        public decimal Discount { get; set; }

        [ValidateNever]
        public int? ProductId { get; set; }

        [ValidateNever]
        public bool isDeleted { get; set; } = false;


        [ValidateNever]
        [NotMapped]
        public ProductDTO? Product { get; set; }
    }
}
