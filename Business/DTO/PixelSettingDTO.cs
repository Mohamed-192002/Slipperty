namespace Business.DTO
{
    public class PixelSettingDTO
    {
        public int? Id { get; set; }
        [Display(Name = "facebook")]
        //[Required(ErrorMessage = "ادخل الاسم")]
        public string? facebook { get; set; }
        //[Required]
        [Display(Name = "tiktok")]
        public string? tiktok { get; set; }
        [Display(Name = "clarity")]
        public string? clarity { get; set; }
    }
}
