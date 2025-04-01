using Infrastructure.Migrations.Models;

namespace Infrastructure.Models
{
    public class OrderHead : IModelBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public DateTime RegDate { get; set; }

        [Required]
        [Precision(18,2)]
        public decimal Total { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Discount { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal NetAmount { get; set; }

        [Required]
        public string? orderNo { get; set; }


        [Required]
        public string? fullName { get; set; }
        [Required]
        public int? paymentMethodId { get; set; }
        [Required]
        public int? governmentId { get; set; }
        //[Required]
        public int? regionId { get; set; }
        public string? deliveryTimeFrom { get; set; }
        public string? deliveryTimeTo { get; set; }


        [Required]
        public string? Address { get; set; }
        [Required]
        public string? phoneNumber { get; set; }
        public string? phoneNumber2 { get; set; }

        public string? Notes { get; set; }
        public int? StatusId { get; set; }  
        public virtual IEnumerable<OrderDetails>? OrderDetails { get; set; }

        [ForeignKey(nameof(governmentId))]
        public Government? Government { get; set; }
        [ForeignKey(nameof(regionId))]
        public Region? Region { get; set; }
        [ForeignKey(nameof(paymentMethodId))]
        public UserPaymentMethod? PaymentMethod { get; set; }

        public OrderUrgentDetails?  OrderUrgentDetails { get; set; }
        public ICollection<OrderHoldingDetails>?  OrderHoldingDetails { get; set; }

        public virtual ICollection<OrderFollowUps>? OrderFollowUps { get; set; }
        public virtual OrderModificationDeclined? OrderModificationDeclined { get; set; } 
        // [ForeignKey(nameof(StatusId))]
        // public virtual OrderStatus?  OrderStatus { get; set; } = null; 

        [NotMapped]
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [NotMapped]
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
