namespace Business.DTO
{
    public class ProductViewDTO
    {
        public IEnumerable<int>? Sizes { get; set; }
        public IEnumerable<CategoryDTO>? Categories { get; set; }
        public IEnumerable<ProductDTO>? Products { get; set; }


        [Display(Name = "الخامة")]
        [ValidateNever]
        public int MaterialId { get; set; }
        [Display(Name = "اللون")]
        [ValidateNever]
        public int ColorId { get; set; }
        [Display(Name = "النوع")]
        [ValidateNever]
        public int ProductTypeId { get; set; }
        [Display(Name = "من")]
        [ValidateNever]
        [Precision(18,2)]
        public decimal priceFrom { get; set; }
        [Display(Name = "الى")]
        [ValidateNever]
        [Precision(18, 2)]
        public decimal priceTo { get; set; }
        [Display(Name = "الماركة")]
        [ValidateNever]
        public int BrandId { get; set; }
        [Display(Name = "التصنيع")]
        [ValidateNever]
        public int ManufacturingId { get; set; }
    }
}
