namespace Business.DTO
{
    public class BlockedNumberDTO
    {
        public int? Id { get; set; }
        [Display(Name = "الرقم")]
        [Required(ErrorMessage = "ادخل الرقم")]
        public string? PhoneNumber { get; set; }
        
    }
}
