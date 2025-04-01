namespace Infrastructure.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? ArbName { get; set; }
        [Required]
        public string? EngName { get; set; }
        [Required, Precision(18, 2)]
        public decimal Price { get; set; }
        [Required, Precision(18, 2)]
        public decimal DiscountPrice { get; set; }
        //[Required]
        //public int? CategoryId { get; set; }
        //[Required]
        //public int? SubCategoryId { get; set; }
        [Required]
        public string? ArbDescription { get; set; }
        [Required]
        public string? EngDescription { get; set; }

        public virtual IEnumerable<ProductImage>? ProductImages { get; set; }
        //[Required]
        public string? MainImageUrl { get; set; }
        //[Required]
        public string? IconImageUrl { get; set; }

        //public bool ViewInLandingPage { get; set; }
        public bool ReneweInWarehouse { get; set; }
        public bool CustomersReviews { get; set; }
        public bool HideCartButton { get; set; }
        public bool Exchargeable { get; set; }
        public bool Returnable { get; set; }
        public bool FreeShipping { get; set; }
        public bool Reviews { get; set; }
        public int VisitorsCount { get; set; }
        public DateTime RegDate { get; set; }
        public int StockCount { get; set; }
        public bool TrackStock { get; set; }
        //public bool StopWhenNoStock { get; set; }



        //public virtual IEnumerable<ProductColor>? ProductColors { get; set; }
        //public virtual IEnumerable<ProductSize>? ProductSizes { get; set; }


        public bool EnableProductCountsOffers { get; set; }
        public virtual IEnumerable<ProductCountsOffer>? ProductCountsOffers { get; set; }


        public bool EnableRelatedProducts { get; set; }
        //public virtual IEnumerable<RelatedProduct>? RelatedProducts { get; set; }


        public int? RelatedProductId { get; set; }

        //public bool EnableProductVideos { get; set; }
        public virtual IEnumerable<ProductVideo>? ProductVideos { get; set; }

        //public bool EnableProductVariables { get; set; }
        public virtual IEnumerable<ProductVariable>? ProductVariables { get; set; }
        public virtual IEnumerable<VariableCombination>? VariableCombinations { get; set; }

        //[ForeignKey(nameof(CategoryId))]
        //public virtual Category Category { get; set; }

        //[ForeignKey(nameof(SubCategoryId))]
        //public virtual SubCategory SubCategory { get; set; }

        //[ForeignKey(nameof(MainImageId))]
        //public virtual ProductImage MainImage { get; set; }
        //[ForeignKey(nameof(IconImageId))]
        //public virtual ProductImage IconImage { get; set; }


        public virtual IEnumerable<ProductCategory>? Categories { get; set; }
        public virtual IEnumerable<ProductSubCategory>? SubCategories { get; set; }

        //[Required]
        public int? ProductTypeId { get; set; }
        [ForeignKey(nameof(ProductTypeId))]
        public virtual ProductType? ProductType { get; set; }
        public int? MaterialId { get; set; }
        [ForeignKey(nameof(MaterialId))]
        public virtual Material? Material { get; set; }
        public int? BrandId { get; set; }
        [ForeignKey(nameof(ManufacturingId))]
        public virtual Brand? Brand { get; set; }
        public int? ManufacturingId { get; set; }
        [ForeignKey(nameof(ManufacturingId))]
        public virtual Manufacturing? Manufacturing { get; set; }
        [ForeignKey(nameof(RelatedProductId))]
        public virtual Product? RelatedProduct { get; set; }


        public int? Duration { get; set; }
        public bool isDeleted { get; set; } = false;

    }
}
