namespace Infrastructure.Models
{
    public class ShoppingCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int CombinationId { get; set; }
        //[Required]
        public string? UserId { get; set; }
        public string? GuestSessionId { get; set; } // Added field for guest session
        [Required]
        public int Qty { get; set; }
        public int? offerId { get; set; }
        [Required]
        public DateTime RegDate { get; set; }


        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        [ForeignKey(nameof(CombinationId))]
        public VariableCombination? Combination { get; set; }
        [ForeignKey(nameof(offerId))]
        public ProductCountsOffer? ProductCountsOffer { get; set; }
    }
}
