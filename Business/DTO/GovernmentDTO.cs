namespace Business.DTO
{
    public class GovernmentDTO
    {
        public int? Id { get; set; }
        [Display(Name = "الاسم")]
        [Required(ErrorMessage = "ادخل الاسم")]
        public string? Name { get; set; }
    }
}
