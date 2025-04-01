namespace Infrastructure.Repositories
{
    public class ProductCategoriesRepository : BaseRepository<ProductCategory>, IProductCategoriesRepository
    {
        private ApplicationDbContext _db;
        public ProductCategoriesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
