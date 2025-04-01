namespace Infrastructure.Models
{
    public class Region
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        [Required]
        public int GovernmentId { get; set; }

        [ForeignKey(nameof(GovernmentId))]
        public Government Government { get; set; }
    }
}
