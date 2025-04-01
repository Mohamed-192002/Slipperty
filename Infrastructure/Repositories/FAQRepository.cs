namespace Infrastructure.Repositories
{
    public class FAQRepository : BaseRepository<FAQ>, IFAQRepository
    {
        private ApplicationDbContext _db;
        public FAQRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
