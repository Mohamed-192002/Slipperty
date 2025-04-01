namespace Infrastructure.Repositories
{
    public class LinkRepository : BaseRepository<Link>, ILinkRepository
    {
        private ApplicationDbContext _db;
        public LinkRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
