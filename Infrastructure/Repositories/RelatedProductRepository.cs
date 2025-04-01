namespace Infrastructure.Repositories
{
    public class RelatedProductRepository : BaseRepository<RelatedProduct>, IRelatedProductRepository
    {
        private ApplicationDbContext _db;
        public RelatedProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
