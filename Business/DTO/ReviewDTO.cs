namespace Business.DTO
{
    public class ReviewDTO
    {
        public int? Id { get; set; }
        [Display(Name = "الاسم")]
        [Required(ErrorMessage = "ادخل الاسم")]
        public string? Name { get; set; }
        [Display(Name = "التاريخ")]
        public DateTime Date { get; set; } = DateTime.Now;
        [Display(Name = "التعليق")]
        [Required(ErrorMessage = "ادخل التعليق")]
        public string? Comment { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? RegDate { get; set; }

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

        public string FormattedDate
        {
            get
            {
                // Use Arabic culture
                var arabicCulture = new System.Globalization.CultureInfo("ar-SA");

                // Format the date as day and month only (e.g., "22 يناير")
                arabicCulture.DateTimeFormat.Calendar = new System.Globalization.GregorianCalendar();

                return Date.ToString("dd MMMM", arabicCulture);
            }
        }
    }
}
