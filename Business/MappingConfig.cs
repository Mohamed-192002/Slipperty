using Business.DTO;
using Infrastructure.Models;

namespace Business
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<SubCategory, SubCategoryDTO>()
                .ForMember(c => c.Category, opt => opt.MapFrom(src => src.Category));
            CreateMap<SubCategoryDTO, SubCategory>();

            CreateMap<int?, int>()
    .ConvertUsing(src => src ?? 0);

            CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.ProductCountsOffers, opt => opt.MapFrom(src => src.ProductCountsOffers))
            //.ForMember(dest => dest.RelatedProduct, opt => opt.MapFrom(src => src.RelatedProduct))
            .ForMember(dest => dest.ProductVideos, opt => opt.MapFrom(src => src.ProductVideos))
            .ForMember(dest => dest.ProductVariables, opt => opt.MapFrom(src => src.ProductVariables))
            .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages))
            .ForMember(dest => dest.ProductImagesOrder, opt => opt.MapFrom(src => src.ProductImages))
            .ForMember(dest => dest.ProductVariableValues, opt => opt.MapFrom(src => src.ProductVariables.SelectMany(pv => pv.VariableValues)))
            .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories))
            .ForMember(dest => dest.SelectedCategories, opt => opt.MapFrom(src => src.Categories.Where(c => c.CategoryId.HasValue).Select(c => c.CategoryId)))
            .ForMember(dest => dest.SelectedSubCategories, opt => opt.MapFrom(src => src.SubCategories.Where(c => c.SubCategoryId.HasValue).Select(c => c.SubCategoryId)))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));

            // Mapping ProductDTO to Product
            CreateMap<ProductDTO, Product>()
                //.ForMember(dest => dest.MainImageId, opt => opt.MapFrom(src => src.MainImageId))
                //.ForMember(dest => dest.IconImageId, opt => opt.MapFrom(src => src.IconImageId))
                //.ForMember(dest => dest.ProductColors, opt => opt.MapFrom(src => src.ProductColors))
                //.ForMember(dest => dest.ProductSizes, opt => opt.MapFrom(src => src.ProductSizes))
                .ForMember(dest => dest.ProductCountsOffers, opt => opt.MapFrom(src => src.ProductCountsOffers))
                .ForMember(dest => dest.RelatedProduct, opt => opt.MapFrom(src => src.RelatedProduct))
                .ForMember(dest => dest.ProductVideos, opt => opt.MapFrom(src => src.ProductVideos))
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src => src.SubCategories))
                .ForMember(dest => dest.VariableCombinations, opt => opt.MapFrom(src => src.VariableCombinations))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType))
                .ForMember(dest => dest.Material, opt => opt.MapFrom(src => src.Material))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand))
                .ForMember(dest => dest.Manufacturing, opt => opt.MapFrom(src => src.Manufacturing))
                .ForMember(dest => dest.ProductVariables, opt => opt.MapFrom(src => src.ProductVariables));

            // Mapping ProductImage to ProductImageDTO
            CreateMap<ProductImage, ProductImageDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder));

            // Mapping ProductImageDTO to ProductImage
            CreateMap<ProductImageDTO, ProductImage>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder));
            
            CreateMap<ProductImage, ProductImageOrderDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder));

            // Mapping ProductCountsOffer to ProductCountsOfferDTO
            CreateMap<ProductCountsOffer, ProductCountsOfferDTO>();

            // Mapping ProductCountsOfferDTO to ProductCountsOffer
            CreateMap<ProductCountsOfferDTO, ProductCountsOffer>();

            // Mapping ProductColor to ProductColorDTO
            CreateMap<ProductColor, ProductColorDTO>();

            // Mapping ProductColorDTO to ProductColor
            CreateMap<ProductColorDTO, ProductColor>();

            // Mapping ProductSize to ProductSizeDTO
            CreateMap<ProductSize, ProductSizeDTO>();

            // Mapping ProductSizeDTO to ProductSize
            CreateMap<ProductSizeDTO, ProductSize>();

            // Mapping RelatedProduct to RelatedProductDTO
            CreateMap<RelatedProduct, RelatedProductDTO>()
                .ForMember(dest => dest.RProduct, opt => opt.MapFrom(src => src.RProduct));

            // Mapping RelatedProductDTO to RelatedProduct
            CreateMap<RelatedProductDTO, RelatedProduct>();

            // Mapping ProductVideo to ProductVideoDTO
            CreateMap<ProductVideo, ProductVideoDTO>();

            // Mapping ProductVideoDTO to ProductVideo
            CreateMap<ProductVideoDTO, ProductVideo>();

            // Mapping ProductVariable to ProductVariableDTO
            CreateMap<ProductVariable, ProductVariableDTO>();

            // Mapping ProductVariableDTO to ProductVariable
            CreateMap<ProductVariableDTO, ProductVariable>();


            CreateMap<ProductCategory, ProductCategoryDTO>();
            CreateMap<ProductCategoryDTO, ProductCategory>();

            CreateMap<ProductSubCategory, ProductSubCategoryDTO>();
            CreateMap<ProductSubCategoryDTO, ProductSubCategory>();

            CreateMap<Brand, BrandDTO>();
            CreateMap<BrandDTO, Brand>();

            CreateMap<Material, MaterialDTO>();
            CreateMap<MaterialDTO, Material>();

            CreateMap<Manufacturing, ManufacturingDTO>();
            CreateMap<ManufacturingDTO, Manufacturing>();

            CreateMap<FAQ, FAQDTO>();
            CreateMap<FAQDTO, FAQ>();


            CreateMap<VariableValue, VariableValueDTO>();
            CreateMap<VariableValueDTO, VariableValue>();

            CreateMap<VariableCombination, VariableCombinationDTO>();
            CreateMap<VariableCombinationDTO, VariableCombination>();

            CreateMap<ProductType, ProductTypeDTO>();
            CreateMap<ProductTypeDTO, ProductType>();

            CreateMap<BlockedNumber, BlockedNumberDTO>();
            CreateMap<BlockedNumberDTO, BlockedNumber>();

            CreateMap<Banner, BannerDTO>();
            CreateMap<BannerDTO, Banner>();

            CreateMap<LinkType, LinkTypeDTO>();
            CreateMap<LinkTypeDTO, LinkType>();
            CreateMap<ModifyOrderDetailDataDto, OrderHead>().ReverseMap();


            CreateMap<Link, LinkDTO>()
                .ForMember(c => c.LinkType, opt => opt.MapFrom(src => src.LinkType));
            CreateMap<LinkDTO, Link>();


            CreateMap<Review, ReviewDTO>();
            CreateMap<ReviewDTO, Review>();
            
            CreateMap<Government, GovernmentDTO>();
            CreateMap<GovernmentDTO, Government>();
            
            CreateMap<Region, RegionDTO>()
                .ForMember(c => c.Government, opt => opt.MapFrom(src => src.Government));
            CreateMap<RegionDTO, Region>();


            CreateMap<PaymentMethod, PaymentMethodDTO>();
            CreateMap<PaymentMethodDTO, PaymentMethod>();

            CreateMap<AddressType, AddressTypeDTO>();
            CreateMap<AddressTypeDTO, AddressType>();


            CreateMap<UserAddress, UserAddressDTO>()
                .ForMember(c => c.Government, opt => opt.MapFrom(src => src.Government))
                .ForMember(c => c.Region, opt => opt.MapFrom(src => src.Region))
                .ForMember(c => c.AddressType, opt => opt.MapFrom(src => src.AddressType));
            CreateMap<UserAddressDTO, UserAddress>();

            CreateMap<UserPaymentMethod, UserPaymentMethodDTO>()
                .ForMember(c => c.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod));
            CreateMap<UserPaymentMethodDTO, UserPaymentMethod>();

            CreateMap<ShoppingCart, ShoppingCartDTO>()
                .ForMember(c => c.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(c => c.ProductCountsOffer, opt => opt.MapFrom(src => src.ProductCountsOffer))
                .ForMember(c => c.Combination, opt => opt.MapFrom(src => src.Combination));
            CreateMap<ShoppingCartDTO, ShoppingCart>();
            
            CreateMap<OrderDetails, OrderDetailsDTO>()
                .ForMember(c => c.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(c => c.ProductCountsOffer, opt => opt.MapFrom(src => src.ProductCountsOffer))
                .ForMember(c => c.Combination, opt => opt.MapFrom(src => src.Combination));
            
            
            CreateMap<OrderDetailsDTO, OrderDetails>();


            CreateMap<OrderHead, OrderHeadDTO>()
                .ForMember(dto => dto.Region, opt => opt.MapFrom(src => src.Region))
                .ForMember(dto => dto.Government, opt => opt.MapFrom(src => src.Government))
                .ForMember(dto => dto.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dto => dto.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
                .ForMember(dto => dto.FollowUpDetails, opt => opt.MapFrom(src => src.OrderFollowUps)); 
            CreateMap<OrderHeadDTO, OrderHead>();


            CreateMap<PixelSetting, PixelSettingDTO>();
            CreateMap<PixelSettingDTO, PixelSetting>();

        }
    }
}
