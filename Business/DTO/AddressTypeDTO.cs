namespace Business.DTO
{
    public class AddressTypeDTO
    {
        public int? Id { get; set; }
        [Display(Name = "الاسم")]
        [Required(ErrorMessage = "ادخل الاسم")]
        public string? Name { get; set; }
        [Required]
        public string Icon { get; set; }
    }
}
