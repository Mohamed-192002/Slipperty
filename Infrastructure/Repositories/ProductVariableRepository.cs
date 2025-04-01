namespace Infrastructure.Repositories
{
    public class ProductVariableRepository : BaseRepository<ProductVariable>, IProductVariableRepository
    {
        private ApplicationDbContext _db;
        public ProductVariableRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
