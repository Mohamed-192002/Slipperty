namespace Infrastructure.Models
{
    public class OrderDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int CombinationId { get; set; }
        [Required]
        public int Qty { get; set; }
        public int? offerId { get; set; }
        [Required]
        public DateTime RegDate { get; set; }
        [Required]
        public string? UserId { get; set; }


        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
        [ForeignKey(nameof(CombinationId))]
        public  VariableCombination? Combination { get; set; }
        [ForeignKey(nameof(offerId))]
        public  ProductCountsOffer? ProductCountsOffer { get; set; }

        [Required]
        public int OrderHeadId { get; set; }

        [ForeignKey(nameof(OrderHeadId))]
        public  OrderHead? OrderHead { get; set; }

    }
}
