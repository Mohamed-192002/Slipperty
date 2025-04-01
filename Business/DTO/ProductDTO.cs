using AutoMapper.Configuration.Annotations;
using Infrastructure.Models;

namespace Business.DTO
{
    public class ProductDTO
    {
        public int? Id { get; set; }
        [Display(Name = "اسم المنتج")]
        [Required(ErrorMessage = "ادخل اسم المنتج")]
        public string? ArbName { get; set; }
        [Display(Name = "اسم المنتج انجليزي")]
        [Required(ErrorMessage = "ادخل اسم المنتج انجليزي")]
        public string? EngName { get; set; }
        [Display(Name = "السعر")]
        [Required(ErrorMessage = "ادخل السعر")]
        [Precision(18,2)]
        public decimal? Price { get; set; }
        [Display(Name = "السعر بعد الخصم")]
        [Required(ErrorMessage = "السعر بعد الخصم")]
        [Precision(18,2)]
        public decimal? DiscountPrice { get; set; }
        //[Display(Name = "تصنيف المنتج")]
        //[Required(ErrorMessage = "اختر تصنيف المنتج")]
        //public int? CategoryId { get; set; }
        //[Display(Name = "التصنيف الفرعي")]
        //[Required(ErrorMessage = "اختر التصنيف الفرعي")]
        //public int? SubCategoryId { get; set; }
        [Display(Name = "وصف المنتج عربي")]
        [Required(ErrorMessage = "ادخل وصف المنتج عربي")]
        public string? ArbDescription { get; set; }
        [Display(Name = "وصف المنتج انجليزي")]
        [Required(ErrorMessage = "ادخل وصف المنتج انجليزي")]
        public string? EngDescription { get; set; }
        
        [Display(Name = "التصنيفات")]
        public virtual List<ProductCategoryDTO>? Categories { get; set; }
        
        [Display(Name = "التصنيفات الفرعية")]
        public virtual List<ProductSubCategoryDTO>? SubCategories { get; set; } 



        //[Required(ErrorMessage = "اختر الصورة الرئيسية")]
        public string? MainImageUrl { get; set; }
        //[Required(ErrorMessage = "اختر ايقونة اللاندينج بيج")]
        public string? IconImageUrl { get; set; }

        //[Display(Name = "عرض صفحة اللاندينج بيج ")]
        //public bool ViewInLandingPage { get; set; }
        [Display(Name = "تجديد المنتج فى المخزن")]
        public bool ReneweInWarehouse { get; set; }
        [Display(Name = "اراء العملاء")]
        public bool CustomersReviews { get; set; }
        [Display(Name = "اخفاء زر العربة")]
        public bool HideCartButton { get; set; }
        [Display(Name = "استبدال المنتج")]
        public bool Exchargeable { get; set; } = true;
        [Display(Name = "ارجاع المنتج")]
        public bool Returnable { get; set; }
        [Display(Name = "شحن مجانا ؟ ")]
        public bool FreeShipping { get; set; }
        [Display(Name = "التقييمات")]
        public bool Reviews { get; set; }
        [Display(Name = "عداد الزوار")]
        public int VisitorsCount { get; set; } = 99;
        [ValidateNever]
        public DateTime? RegDate { get; set; }
        [Display(Name = "عدد القطع المتبقية")]
        public int StockCount { get; set; }
        [Display(Name = "تتبع المخزون")]
        public bool TrackStock { get; set; }
        //[Display(Name = "تعديل المنتج عند نفاذ المخزون")]
        //public bool StopWhenNoStock { get; set; }

        //[ValidateNever]
        //[NotMapped]
        [Display(Name = "مده العرض")]
        public int? Duration { get; set; } = 0;

        //public virtual IEnumerable<ProductColorDTO>? ProductColors { get; set; }
        //public virtual IEnumerable<ProductSizeDTO>? ProductSizes { get; set; }

        [Display(Name = "تفعيل عرض القطع")]
        public bool EnableProductCountsOffers { get; set; }
        public virtual IEnumerable<ProductCountsOfferDTO>? ProductCountsOffers { get; set; }


        [Display(Name = "تفعيل المنتجات المتعلقة")]
        public bool EnableRelatedProducts { get; set; }
        //public virtual IEnumerable<RelatedProductDTO>? RelatedProducts { get; set; }

        //[Display(Name = "تفعيل فيديوهات المنتج")]
        //public bool EnableProductVideos { get; set; }
        public virtual IEnumerable<ProductVideoDTO>? ProductVideos { get; set; }

        //[Display(Name = "تفعيل متغيرات المنتج")]
        //public bool EnableProductVariables { get; set; }
        public virtual IEnumerable<ProductVariableDTO>? ProductVariables { get; set; }
        public virtual IEnumerable<VariableValueDTO>? ProductVariableValues { get; set; }
        public virtual IEnumerable<VariableCombinationDTO>? VariableCombinations { get; set; }
        //[ValidateNever]
        public virtual List<ProductImageDTO>? ProductImages { get; set; }

        [ValidateNever]
        [NotMapped]
        public IEnumerable<ProductImageOrderDTO>? ProductImagesOrder { get; set; }


        [ValidateNever]
        [NotMapped]
        public IEnumerable<ReviewDTO>? ProductReviews { get; set; }

        //[ValidateNever]
        //[NotMapped]
        //public virtual CategoryDTO? Category { get; set; }
        //[ValidateNever]
        //[NotMapped]
        //public virtual SubCategoryDTO? SubCategory { get; set; }
        //[ValidateNever]
        //[NotMapped]
        //public virtual ProductImageDTO? MainImage { get; set; }
        //[ValidateNever]
        //[NotMapped]
        //public virtual ProductImageDTO? IconImage { get; set; }


        [ValidateNever]
        [NotMapped]
        [Display(Name = "الاسم")]
        public string? VariableName { get; set; }
        
        [ValidateNever]
        [NotMapped]
        [Display(Name = "العدد")]
        public string? OfferCount { get; set; }
        [ValidateNever]
        [NotMapped]
        [Display(Name = "السعر")]
        public string? OfferPrice { get; set; }

        [ValidateNever]
        //[NotMapped]
        [Display(Name = "المنتجات المتعلقة")]
        public int? RelatedProductId { get; set; }
        
        [ValidateNever]
        [NotMapped]
        [Display(Name = "رابط الفيديو")]
        public int? VideoUrl { get; set; }

        [ValidateNever]
        [NotMapped]
        public virtual List<int>? SelectedCategories { get; set; }
        [ValidateNever]
        [NotMapped]
        public virtual List<int>? SelectedSubCategories { get; set; }

        [Required(ErrorMessage = "اختر نوع الوش")]
        [Display(Name = "نوع الوش")]
        public int? ProductTypeId { get; set; }

        [ValidateNever]
        [NotMapped]
        public ProductTypeDTO? ProductType { get; set; }
        [Required(ErrorMessage = "اختر الخامة")]
        [Display(Name = "الخامة")]
        public int? MaterialId { get; set; }

        [ValidateNever]
        [NotMapped]
        public MaterialDTO? Material { get; set; }
        [Required(ErrorMessage = "اختر الماركة")]
        [Display(Name = "الماركة")]
        public int? BrandId { get; set; }

        [ValidateNever]
        [NotMapped]
        public BrandDTO? Brand { get; set; }
        [ValidateNever]
        [NotMapped]
        public ProductDTO? RelatedProduct { get; set; }


        [Required(ErrorMessage = "اختر التصنيع")]
        [Display(Name = "التصنيع")]
        public int? ManufacturingId { get; set; }
        [ValidateNever]
        [NotMapped]
        public Manufacturing? Manufacturing { get; set; }


        [ValidateNever]
        [NotMapped]
        public IEnumerable<int>? Sizes { get; set; }
        [ValidateNever]
        [NotMapped]
        public IEnumerable<string>? Colors { get; set; }


        [ValidateNever]
        [NotMapped]
        public IEnumerable<ProductDTO>? ViewRelatedProducts { get; set; }
        [ValidateNever]
        [NotMapped]
        public IEnumerable<ProductDTO>? HighlyRated { get; set; }

        [ValidateNever]
        [NotMapped]
        [Display(Name = "الاسم")]
        public string? NewBrandValue { get; set; }
        [ValidateNever]
        [NotMapped]
        [Display(Name = "الاسم")]
        public string? NewMaterialValue { get; set; }
        [ValidateNever]
        [NotMapped]
        [Display(Name = "الاسم")]
        public string? NewProductTypeValue { get; set; }
        [ValidateNever]
        [NotMapped]
        [Display(Name = "الاسم")]
        public string? NewManufacturingValue { get; set; }


        [ValidateNever]
        [NotMapped]
        public IEnumerable<UserAddressDTO>? UserAddressDTOs { get; set; }
        [ValidateNever]
        [NotMapped]
        public IEnumerable<UserPaymentMethodDTO>? UserPaymentMethodDTOs { get; set; }

        [ValidateNever]
        public bool isDeleted { get; set; } = false;

    }
}
