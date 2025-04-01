namespace Business.DTO
{
    public class VariableValueDTO
    {
        public int? Id { get; set; }

        [Required]
        [Display(Name = "قيمة المتغير")]
        public string? Value { get; set; }

        public int ProductVariableId { get; set; }


        [ValidateNever]
        [NotMapped]
        public ProductVariableDTO? ProductVariable { get; set; }

        [ValidateNever]
        [NotMapped]
        public string? VariableName { get; set; }


        public string? ImageUrl { get; set; }
    }
}
