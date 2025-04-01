namespace Business.DTO
{
    public class UserDTO
    {
        public string? Id { get; set; }

        [StringLength(100, ErrorMessage = "يجب الا يقل الاسم الاول عن 3 حروف", MinimumLength = 3)]
        [Display(Name = "الاسم الاول")]
        [Required(ErrorMessage = "ادخل الاسم الأول")]
        public string? FirstName { get; set; }

        [StringLength(100, ErrorMessage = "يجب الا يقل الاسم الثاني عن 3 حروف", MinimumLength = 3)]
        [Display(Name = "الاسم الثاني")]
        [Required(ErrorMessage = "ادخل الاسم الثاني")]
        public string? LastName { get; set; }

        [Display(Name = "رقم الهاتف")]
        [Required(ErrorMessage = "ادخل رقم الهاتف")]
        [StringLength(12, ErrorMessage = "ادخل رقم هاتف صحيح", MinimumLength = 12)]
        //[RegularExpression(@"^0\d{10}$", ErrorMessage = "رقم الهاتف غير صالح. يجب أن يتكون من 12 رقمًا يبدأ بـ20.")]  // Ensures the phone number is 11 digits starting with 0
        public string? PhoneNumber { get; set; }

        [Display(Name = "العنوان")]
        [Required(ErrorMessage = "ادخل العنوان")]
        public string? Address { get; set; }

        [Display(Name = "المحافظة")]
        [Required(ErrorMessage = "ختر المحافظة")]
        public int? GovernmentId { get; set; }

        [Display(Name = "المنطقة")]
        [Required(ErrorMessage = "اختر المنطقة")]
        public int? RegionId { get; set; }

        public string? GovernmentName { get; set; }
        public string? RegionName { get; set; }



        //[Display(Name = "UserName")]
        //[Required(ErrorMessage = "UserNameRequired")]
        //[StringLength(100, ErrorMessage = "UserNameStringLength")]
        //public string? UserName { get; set; }

        [Display(Name = "الباسورد")]
        [Required(ErrorMessage = "ادخل الباسورد")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "اعادة الباسورد")]
        [Compare("Password", ErrorMessage = "الباسورد غير متطابق")]
        public string? ConfirmPassword { get; set; }

        [ValidateNever]
        [NotMapped]
        public IEnumerable<UserPaymentMethodDTO>? UserPaymentMethods { get; set; }
        public IEnumerable<UserAddressDTO>? UserAddresses { get; set; }

        [ValidateNever]
        public bool AutoRegistered { get; set; } = false;
    }
}
