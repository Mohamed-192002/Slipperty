namespace Business.DTO
{
    public class PhoneNumberConfirmationDTO
    {
        [Required]
        public string? OTPCode { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? otp1 { get; set; }
        [Required] 
        public string? otp2 { get; set; }
        [Required] 
        public string? otp3 { get; set; }
        [Required] 
        public string? otp4 { get; set; }
    }
}
