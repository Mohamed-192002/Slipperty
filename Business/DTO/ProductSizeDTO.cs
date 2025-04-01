namespace Business.DTO
{
    public class ProductSizeDTO
    {
        public int? Id { get; set; }
        [Display(Name = "المقاس")]
        [Required(ErrorMessage = "ادخل المقاس")]
        public int? Size { get; set; }
        [Required(ErrorMessage = "عدد القطع")]
        public int? SizeCount { get; set; }

        [Display(Name = "اللون")]
        [Required(ErrorMessage = "اختر اللون")]
        public int? ColorId { get; set; }
        [ValidateNever]
        public int? ProductId { get; set; }

        [NotMapped] 
        public string? ColorName { get; set;}


        [ValidateNever]
        [NotMapped]
        public ProductDTO? Product { get; set; }
    }
}
