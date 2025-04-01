using Infrastructure.Migrations.Models;
using System.Drawing;

namespace Business.DTO
{
    public class OrderHeadDTO
    {
        public int? Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public DateTime RegDate { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Total { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Discount { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal NetAmount { get; set; }

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
        public string? orderNo { get; set; }

        public string? Notes { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? phoneNumber { get; set; }
        public string? phoneNumber2 { get; set; }

        public virtual IEnumerable<OrderDetailsDTO>? OrderDetails { get; set; }
        public virtual ICollection<OrderFollowUps>? FollowUpDetails { get; set; } 
        public virtual OrderModificationDeclined? OrderModificationDeclined { get; set; } 

        [ValidateNever]
        [NotMapped] 
        public UserPaymentMethodDTO? PaymentMethod { get; set; }
        [ValidateNever]
        [NotMapped] 
        public GovernmentDTO? Government { get; set; }
        [ValidateNever]
        [NotMapped] 
        public RegionDTO? Region { get; set; }
        public int? StatusId { get; set; }


        public OrderUrgentDetails? OrderUrgentDetails { get; set; }
        public ICollection<OrderHoldingDetails>?  OrderHoldingDetails { get; set; }
        
        [NotMapped]
        public bool IsHaveMoreThanOneOrder { get; set; }
        
        [NotMapped]
        public OrderCardFilterDto? OrderCardFilter { get; set; }
        
        
        



        //public string TimeAgo
        //{
        //    get
        //    {
        //        if (RegDate == null) return string.Empty;

        //        var now = DateTime.Now;
        //        var regDate = RegDate.Value;
        //        var timeSpan = now - regDate;

        //        int days = (int)timeSpan.TotalDays;
        //        int hours = timeSpan.Hours;
        //        int minutes = timeSpan.Minutes;

        //        var timeParts = new List<string>();

        //        if (days > 0)
        //            timeParts.Add($"{days} يوم");
        //        if (hours > 0)
        //            timeParts.Add($"{hours} ساعة");
        //        if (minutes > 0)
        //            timeParts.Add($"{minutes} دقيقة");

        //        // If no time parts, show "منذ قليل"
        //        return timeParts.Count > 0 ? "منذ " + string.Join(" و ", timeParts) : "منذ قليل";
        //    }
        //}
    }
}
