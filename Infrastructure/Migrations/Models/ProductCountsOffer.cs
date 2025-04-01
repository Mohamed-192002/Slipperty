namespace Infrastructure.Models
{
    public class ProductCountsOffer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int Count { get; set; }
        [Required, Precision(18,2)]
        public decimal Price { get; set; }
        [Required]
        public int? ProductId { get; set; }


        [Required]
        public bool isDeleted { get; set; }  = false;

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }
    }
}
