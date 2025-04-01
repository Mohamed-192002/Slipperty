namespace Infrastructure.Repositories
{
    public class LinkTypeRepository : BaseRepository<LinkType>, ILinkTypeRepository
    {
        private ApplicationDbContext _db;
        public LinkTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
