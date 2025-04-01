namespace Infrastructure.Models
{
    public class UserPaymentMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public string? CardNumber { get; set; }
        public string? CardHolderName { get; set; }
        public string? ExpiryDate { get; set; }
        public string? CVV { get; set; }


        [Required]
        public int PaymentMethodId { get; set; }

        [Required]
        public bool isDefault { get; set; }

        [Required]
        public string? userId { get; set; }

        [ForeignKey(nameof(PaymentMethodId))]
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
