namespace Infrastructure.Models
{
    public class ProductSize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int Size { get; set; }
        [Required]
        public int? ColorId { get; set; }
        [Required]
        public int? ProductId { get; set; }

        [ForeignKey(nameof(ColorId))]
        public virtual ProductColor Color { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}
