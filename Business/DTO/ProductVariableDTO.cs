namespace Business.DTO
{
    public class ProductVariableDTO
    {
        public int? Id { get; set; }
        [Display(Name = "اسم المتغير")]
        [Required(ErrorMessage = "ادخل الاسم")]
        public string? Name { get; set; }

        public virtual IEnumerable<VariableValueDTO>? VariableValues { get; set; }

        [ValidateNever]
        public int? ProductId { get; set; }

        [ValidateNever]
        [NotMapped]
        public ProductDTO? Product { get; set; }


    }
}
