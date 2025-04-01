namespace Business.DTO
{
    public class ProductColorDTO
    {
        public int? Id { get; set; }
        [Display(Name = "الاسم")]
        [Required(ErrorMessage = "ادخل الاسم")]
        public string? Name { get; set; }
        [ValidateNever]
        public int? ProductId { get; set; }


        [ValidateNever]
        [NotMapped]
        public ProductDTO? Product { get; set; }
    }
}
