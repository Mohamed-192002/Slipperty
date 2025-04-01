using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        ISubCategoryRepository SubCategories { get; }
        IProductRepository Products { get; }
        IBrandRepository Brands { get; }
        IManufacturingRepository Manufacturing { get; }
        IMaterialRepository Materials { get; }
        IFAQRepository FAQs { get; }
        IProductCategoriesRepository ProductCategories { get; }
        IProductSubCategoriesRepository ProductSubCategories { get; }
        IProductVideoRepository ProductVideos { get; }
        IRelatedProductRepository RelatedProducts { get; }
        IProductCountsOfferRepository ProductCountsOffers { get; }
        IVariableCombinationRepository VariableCombinations { get; }
        IVariableValueRepository VariableValues { get; }
        IProductVariableRepository ProductVariables { get; }
        IProductImageRepository ProductImages { get; }
        IProductTypeRepository ProductTypes { get; }
        IBlockedNumberRepository BlockedNumbers { get; }
        IBannerRepository Banners { get; }
        ILinkTypeRepository LinkTypes { get; }
        ILinkRepository Links { get; }
        IReviewRepository Reviews { get; }
        IGovernmentRepository Governments { get; }
        IRegionRepository Regions { get; }
        IPaymentMethodRepository PaymentMethods { get; }
        IAddressTypeRepository AddressTypes { get; }
        IUserAddressRepository UserAddresses { get; }
        IUserPaymentMethodRepository UserPaymentMethods { get; }
        IShoppingCartRepository ShoppingCarts { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IOrderHeadRepository OrderHead { get; }
        IPixelSettingRepository PixelSettings { get; }
        IOrderStatusRepository OrderStatus { get; }
        IOrderTransactionRepository OrderTransaction { get;  }
        IOrderFollowUpsRepository OrderFollowUps { get;  }
        IOperationTypeRepository OperationTypes { get; }
        
        IContactMethodRepository ContactMethods { get; }
        
        IContactStatusRepository ContactStatuses { get; }
        
        IOrderCancelationDetailsRepository OrderCancelationDetails { get; }
        IOrderHoldingDetailsRepository OrderHoldingDetailsRepository { get; }
        IOrderUrgentDetailsRepository OrderUrgentDetailsRepository  { get; }
        IOrderModificationDeclinedRepository OrderModificationDeclinedRepository { get; }
        
	    object GetLookUpRepository(string lookUpName);

        IQueryable<T> ExcecuteQuery<T>(string query, params object[] param)  where T : class; 
        
        int Complete();
    }
}
