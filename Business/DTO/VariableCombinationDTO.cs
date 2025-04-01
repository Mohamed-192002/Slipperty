namespace Business.DTO
{
    public class VariableCombinationDTO
    {
        public int? Id { get; set; }
        public string? Text { get; set; }
        [Precision(18,2)]
        [Display(Name = "السعر")]
        public decimal? Price { get; set;}
        [Display(Name = "عدد القطع")]
        public int? StockCount { get; set; }
        [ValidateNever]
        public int? ProductId { get; set; }
        [ValidateNever]
        [NotMapped]
        public ProductDTO? Product { get; set; }



    }
}
