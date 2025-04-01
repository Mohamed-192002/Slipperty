namespace Infrastructure.Models
{
    public class ProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? ImageUrl { get; set; }
        [Required]
        public int? ProductId { get; set; }

        public int DisplayOrder { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}
