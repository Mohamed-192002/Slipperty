namespace Infrastructure.Models
{
    public class Link
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Url { get; set; }
        [Required]
        public DateTime RegDate { get; set; }
        [Required]
        public int? LinkTypeId { get; set; }

        [ForeignKey(nameof(LinkTypeId))]
        public virtual LinkType LinkType { get; set; }
    }
}
