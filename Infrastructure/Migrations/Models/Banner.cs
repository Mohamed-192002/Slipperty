namespace Infrastructure.Models
{
    public class Banner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public DateTime RegDate { get; set; }
    }
}
