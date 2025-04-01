namespace Business.DTO
{
    public class ProductVideoDTO
    {
        public int? Id { get; set; }
        [Display(Name = "الرابط")]
        [Required(ErrorMessage = "ادخل الرابط")]
        public string? VideoUrl { get; set; }
        [ValidateNever]
        public int? ProductId { get; set; }


        [ValidateNever]
        [NotMapped]
        public ProductDTO? Product { get; set; }
    }
}
