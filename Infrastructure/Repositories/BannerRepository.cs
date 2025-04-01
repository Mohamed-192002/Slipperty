namespace Infrastructure.Repositories
{
    public class BannerRepository : BaseRepository<Banner>, IBannerRepository
    {
        private ApplicationDbContext _db;
        public BannerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
