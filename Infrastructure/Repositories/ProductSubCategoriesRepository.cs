namespace Infrastructure.Repositories
{
    public class ProductSubCategoriesRepository : BaseRepository<ProductSubCategory>, IProductSubCategoriesRepository
    {
        private ApplicationDbContext _db;
        public ProductSubCategoriesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
