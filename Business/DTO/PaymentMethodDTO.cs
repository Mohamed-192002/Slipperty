namespace Business.DTO
{
    public class PaymentMethodDTO
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "ادخل الاسم")]
        public string? Name { get; set; }
        public DateTime? RegDate { get; set; }

        // New property to calculate the time ago
        public string TimeAgo
        {
            get
            {
                if (RegDate == null) return string.Empty;

                var now = DateTime.Now;
                var regDate = RegDate.Value;
                var timeSpan = now - regDate;

                int days = (int)timeSpan.TotalDays;
                int hours = timeSpan.Hours;
                int minutes = timeSpan.Minutes;

                var timeParts = new List<string>();

                if (days > 0)
                    timeParts.Add($"{days} يوم");
                if (hours > 0)
                    timeParts.Add($"{hours} ساعة");
                if (minutes > 0)
                    timeParts.Add($"{minutes} دقيقة");

                // If no time parts, show "منذ قليل"
                return timeParts.Count > 0 ? "منذ " + string.Join(" و ", timeParts) : "منذ قليل";
            }
        }
    }
}
