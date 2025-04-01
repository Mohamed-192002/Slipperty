namespace Business.DTO
{
    public class OrderDetailsDTO
    {
        public int? Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        //[Required]
        public int? CombinationId { get; set; }
        [Required]
        public int Qty { get; set; }
        public int? offerId { get; set; }
        [Required]
        public DateTime RegDate { get; set; }
        [Required]
        public string? UserId { get; set; }



        [ValidateNever]
        [NotMapped]
        public ProductDTO? Product { get; set; }
        [ValidateNever]
        [NotMapped]
        public VariableCombinationDTO? Combination { get; set; }
        [ValidateNever]
        [NotMapped]
        public ProductCountsOfferDTO? ProductCountsOffer { get; set; }


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
