namespace Business.DTO
{
    public class UserPaymentMethodDTO
    {
        public int? Id { get; set; }
        [Display(Name = "رقم البطاقة (14 رقم في وجه البطاقة)")]
        [Required(ErrorMessage = "ادخل رقم البطاقة")]
        [StringLength(14, ErrorMessage = "ادخل رقم بطاقة صحيح", MinimumLength = 14)]
        public string? CardNumber { get; set; }
        [Display(Name = "اسم صاحب البطاقة)")]
        [Required(ErrorMessage = "ادخل اسم صاحب البطاقة")]
        public string? CardHolderName { get; set; }
        [Display(Name = "تاريخ الانتهاء)")]
        [Required(ErrorMessage = "ادخل تاريخ الانتهاء")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "تاريخ غير صحيح. يجب أن يكون  MM/YY.")]
        public string? ExpiryDate { get; set; }
        [Display(Name = "الرقم السري)")]
        [Required(ErrorMessage = "الرقم السري")]
        [StringLength(4, ErrorMessage = "ادخل رقم صحيح", MinimumLength = 3)]
        public string? CVV { get; set; }

        [Display(Name = "اجعلها طريقة دفع أساسية)")]
        public bool isDefault { get; set; }

        [Required]
        public string? userId { get; set; }

        [Required]
        public int PaymentMethodId { get; set; } = 2; //Visa


        [ValidateNever]
        [NotMapped]
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
