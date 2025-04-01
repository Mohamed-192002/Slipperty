namespace Business.DTO
{
    public class SetPasswordDTO
    {
        public required string userId { get; set; }

        [Display(Name = "الباسورد")]
        [Required(ErrorMessage = "ادخل الباسورد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "اعادة الباسورد")]
        [Compare("Password", ErrorMessage = "الباسورد غير متطابق")]
        public string ConfirmPassword { get; set; }
        
    }
}
