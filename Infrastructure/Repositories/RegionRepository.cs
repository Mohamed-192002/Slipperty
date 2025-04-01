namespace Infrastructure.Repositories
{
    public class RegionRepository : BaseRepository<Region>, IRegionRepository
    {
        private ApplicationDbContext _db;
        public RegionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
