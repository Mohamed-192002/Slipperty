namespace Infrastructure.Models
{
    public class RelatedProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int? RelatedProductId { get; set; }
        [Required]
        public int? ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        [ForeignKey(nameof(RelatedProductId))]
        public virtual Product RProduct { get; set; }
    }
}
