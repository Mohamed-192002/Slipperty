using Infrastructure.Migrations.Models;

namespace Infrastructure.Contracts
{
    public class UnitOfWork : IUnitOfWork , IDisposable
    {
         private ApplicationDbContext _db;
         private ICategoryRepository _categories;
         private ISubCategoryRepository _subCategories;
         private IProductRepository _products;
         private IBrandRepository _brands;
         private IManufacturingRepository _manufacturing;
         private IMaterialRepository _materials;
         private IFAQRepository _faqs;
         private IProductCategoriesRepository _productCategories;
         private IProductSubCategoriesRepository _productSubCategories;
         private IProductVideoRepository _productVideos;
         private IRelatedProductRepository _relatedProducts;
         private IProductCountsOfferRepository _productCountsOffers;
         private IVariableCombinationRepository _variableCombinations;
         private IVariableValueRepository _variableValues;
         private IProductVariableRepository _productVariables;
         private IProductImageRepository _productImages;
         private IProductTypeRepository _productTypes;
         private IBlockedNumberRepository _blockedNumbers;
         private IBannerRepository _banners;
         private ILinkTypeRepository _linkTypes;
         private ILinkRepository _links;
         private IReviewRepository _reviews;
         private IGovernmentRepository _governments;
         private IRegionRepository _regions;
         private IPaymentMethodRepository _paymentMethods;
         private IAddressTypeRepository _addressTypes;
         private IUserAddressRepository _userAddresses;
         private IUserPaymentMethodRepository _userPaymentMethods;
         private IShoppingCartRepository _shoppingCarts;
         private IOrderDetailsRepository _orderDetails;
         private IOrderHeadRepository _orderHead;
         private IPixelSettingRepository _pixelSettings;
         private IOrderStatusRepository _orderStatus; 
         private IOrderTransactionRepository _orderTransaction ;
         private IOrderFollowUpsRepository _orderFollowUps ;
         private IOperationTypeRepository _operationTypes ;
         private IContactMethodRepository _contactMethods ;
         private IContactStatusRepository _contactStatuses ;
         private IOrderCancelationDetailsRepository _orderCancelationDetails; 
         private IOrderHoldingDetailsRepository _OrderHoldingDetailsRepository; 
         private IOrderUrgentDetailsRepository _orderUrgentDetailsRepository;
         private IOrderModificationDeclinedRepository _orderModificationDeclinedRepository;
    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(_db);
    public ISubCategoryRepository SubCategories => _subCategories ??= new SubCategoryRepository(_db);
    public IProductRepository Products => _products ??= new ProductRepository(_db);
    public IBrandRepository Brands => _brands ??= new BrandRepository(_db);
    public IManufacturingRepository Manufacturing => _manufacturing ??= new ManufacturingRepository(_db);
    public IMaterialRepository Materials => _materials ??= new MaterialRepository(_db);
    public IFAQRepository FAQs => _faqs ??= new FAQRepository(_db);
    public IProductCategoriesRepository ProductCategories => _productCategories ??= new ProductCategoriesRepository(_db);
    public IProductSubCategoriesRepository ProductSubCategories => _productSubCategories ??= new ProductSubCategoriesRepository(_db);
    public IProductVideoRepository ProductVideos => _productVideos ??= new ProductVideoRepository(_db);
    public IRelatedProductRepository RelatedProducts => _relatedProducts ??= new RelatedProductRepository(_db);
    public IProductCountsOfferRepository ProductCountsOffers => _productCountsOffers ??= new ProductCountsOfferRepository(_db);
    public IVariableCombinationRepository VariableCombinations => _variableCombinations ??= new VariableCombinationRepository(_db);
    public IVariableValueRepository VariableValues => _variableValues ??= new VariableValueRepository(_db);
    public IProductVariableRepository ProductVariables => _productVariables ??= new ProductVariableRepository(_db);
    public IProductImageRepository ProductImages => _productImages ??= new ProductImageRepository(_db);
    public IProductTypeRepository ProductTypes => _productTypes ??= new ProductTypeRepository(_db);
    public IBlockedNumberRepository BlockedNumbers => _blockedNumbers ??= new BlockedNumberRepository(_db);
    public IBannerRepository Banners => _banners ??= new BannerRepository(_db);
    public ILinkTypeRepository LinkTypes => _linkTypes ??= new LinkTypeRepository(_db);
    public ILinkRepository Links => _links ??= new LinkRepository(_db);
    public IReviewRepository Reviews => _reviews ??= new ReviewRepository(_db);
    public IGovernmentRepository Governments => _governments ??= new GovernmentRepository(_db);
    public IRegionRepository Regions => _regions ??= new RegionRepository(_db);
    public IPaymentMethodRepository PaymentMethods => _paymentMethods ??= new PaymentMethodRepository(_db);
    public IAddressTypeRepository AddressTypes => _addressTypes ??= new AddressTypeRepository(_db);
    public IUserAddressRepository UserAddresses => _userAddresses ??= new UserAddressRepository(_db);
    public IUserPaymentMethodRepository UserPaymentMethods => _userPaymentMethods ??= new UserPaymentMethodRepository(_db);
    public IShoppingCartRepository ShoppingCarts => _shoppingCarts ??= new ShoppingCartRepository(_db);
    public IOrderDetailsRepository OrderDetails => _orderDetails ??= new OrderDetailsRepository(_db);
        public IOrderHeadRepository OrderHead => _orderHead ??= new OrderHeadRepository(_db);
        public IPixelSettingRepository PixelSettings => _pixelSettings ??= new PixelSettingRepository(_db);
        public IOrderStatusRepository OrderStatus => _orderStatus ??= new OrderStatusRepositoryRepository(_db);
        public IOrderTransactionRepository OrderTransaction => _orderTransaction ??= new OrderTransactionRepositoryRepository(_db);
        public IOrderFollowUpsRepository OrderFollowUps => _orderFollowUps ??= new OrderFollowUpsRepositoryRepository(_db);
        public IOperationTypeRepository OperationTypes => _operationTypes ??= new OperationTypeRepositoryRepository(_db);
        public IContactMethodRepository ContactMethods => _contactMethods ??= new ContactMethodRepositoryRepository(_db);
        public IContactStatusRepository ContactStatuses => _contactStatuses ??= new ContactStatusRepositoryRepository(_db);

        public IOrderCancelationDetailsRepository OrderCancelationDetails =>
            _orderCancelationDetails ??= new OrderCancelationDetailsRepository(_db);
        public IOrderHoldingDetailsRepository OrderHoldingDetailsRepository =>
                _OrderHoldingDetailsRepository ??= new OrderHoldingDetailsRepository(_db);

        public IOrderModificationDeclinedRepository OrderModificationDeclinedRepository =>
            _orderModificationDeclinedRepository ??= new OrderModificationDeclinedRepository(_db);
        public IOrderUrgentDetailsRepository OrderUrgentDetailsRepository =>
            _orderUrgentDetailsRepository ??= new OrderUrgentDetailsRepository(_db);


        // public UnitOfWork(ApplicationDbContext db)
        // {
        //     _db = db;
        //     Categories = new CategoryRepository(_db);
        //     SubCategories = new SubCategoryRepository(_db);
        //     Products = new ProductRepository(_db);
        //     Brands = new BrandRepository(_db);
        //     Manufacturing = new ManufacturingRepository(_db);
        //     Materials = new MaterialRepository(_db);
        //     FAQs = new FAQRepository(_db);
        //     ProductCategories = new ProductCategoriesRepository(_db);
        //     ProductSubCategories = new ProductSubCategoriesRepository(_db);
        //     ProductVideos = new ProductVideoRepository(_db);
        //     RelatedProducts = new RelatedProductRepository(_db);
        //     ProductCountsOffers = new ProductCountsOfferRepository(_db);
        //     VariableCombinations = new VariableCombinationRepository(_db);
        //     VariableValues = new VariableValueRepository(_db);
        //     ProductVariables = new ProductVariableRepository(_db);
        //     ProductImages = new ProductImageRepository(_db);
        //     ProductTypes = new ProductTypeRepository(_db);
        //     BlockedNumbers = new BlockedNumberRepository(_db);
        //     Banners = new BannerRepository(_db);
        //     LinkTypes = new LinkTypeRepository(_db);
        //     Links = new LinkRepository(_db);
        //     Reviews = new ReviewRepository(_db);
        //     Governments = new GovernmentRepository(_db);
        //     Regions = new RegionRepository(_db);
        //     PaymentMethods = new PaymentMethodRepository(_db);
        //     AddressTypes = new AddressTypeRepository(_db);
        //     UserAddresses = new UserAddressRepository(_db);
        //     UserPaymentMethods = new UserPaymentMethodRepository(_db);
        //     ShoppingCarts = new ShoppingCartRepository(_db);
        //     OrderHead = new OrderHeadRepository(_db);
        //     OrderDetails = new OrderDetailsRepository(_db);
        //     PixelSettings = new PixelSettingRepository(_db);
        //
        // }

        public  IQueryable<T> ExcecuteQuery<T>(string query, params object[]? param)  where T : class
        {
            return _db.Set<T>().FromSqlRaw(query, param).AsNoTracking();

        }
        public object GetLookUpRepository(string lookUpName)
        {
            var Repo = Activator.CreateInstance(typeof(BaseRepository<>).MakeGenericType(LookupTables.LookUpsTable[lookUpName]),_db); 	
            return Repo; 
        }




        public int Complete()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
    
   
}
