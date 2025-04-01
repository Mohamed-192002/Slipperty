namespace Infrastructure.Models
{
    public class PixelSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[Required]
        public string? facebook { get; set; }
        //[Required]
        public string? tiktok { get; set; }
        //[Required]
        public string? clarity { get; set; }
    }
}
