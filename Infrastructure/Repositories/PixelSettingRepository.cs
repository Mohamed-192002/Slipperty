namespace Infrastructure.Repositories
{
    public class PixelSettingRepository : BaseRepository<PixelSetting>, IPixelSettingRepository
    {
        private ApplicationDbContext _db;
        public PixelSettingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
