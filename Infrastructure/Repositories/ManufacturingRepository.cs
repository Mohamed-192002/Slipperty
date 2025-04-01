namespace Infrastructure.Repositories
{
    public class ManufacturingRepository : BaseRepository<Manufacturing>, IManufacturingRepository
    {
        private ApplicationDbContext _db;
        public ManufacturingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
