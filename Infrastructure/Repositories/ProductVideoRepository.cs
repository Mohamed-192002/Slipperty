namespace Infrastructure.Repositories
{
    public class ProductVideoRepository : BaseRepository<ProductVideo>, IProductVideoRepository
    {
        private ApplicationDbContext _db;
        public ProductVideoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
