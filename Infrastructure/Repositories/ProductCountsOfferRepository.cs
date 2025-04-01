namespace Infrastructure.Repositories
{
    public class ProductCountsOfferRepository : BaseRepository<ProductCountsOffer>, IProductCountsOfferRepository
    {
        private ApplicationDbContext _db;
        public ProductCountsOfferRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
