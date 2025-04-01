namespace Business.DTO
{
    public class RegisterPhoneNumberDTO
    {
        [Required(ErrorMessage = "ادخل رقم الهاتف")]
        [Display(Name = "رقم الهاتف")]
        [RegularExpression(@"^0\d{10}$", ErrorMessage = "رقم الهاتف غير صالح. يجب أن يتكون من 11 رقمًا يبدأ بـ 0.")]  // Ensures the phone number is 11 digits starting with 0
        public string? PhoneNumber { get; set; }
    }
}
