namespace Infrastructure.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
        public string? ImageUrl { get; set; }
        [Required]
        public DateTime RegDate { get; set; }
    }
}
