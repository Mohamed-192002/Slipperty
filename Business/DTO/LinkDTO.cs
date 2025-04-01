namespace Business.DTO
{
    public class LinkDTO
    {
        public int? Id { get; set; }
        [Display(Name = "الرابط")]
        [Required(ErrorMessage = "ادخل الرابط")]
        public string? Url { get; set; }
        public DateTime? RegDate { get; set; }
        [Display(Name = "النوع الاساسي")]
        [Required(ErrorMessage = "اختر النوع")]
        public int? LinkTypeId { get; set; }

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

        [ValidateNever]
        [NotMapped]
        public LinkTypeDTO? LinkType { get; set; }
    }
}
