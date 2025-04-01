namespace Infrastructure.Repositories
{
    public class BlockedNumberRepository : BaseRepository<BlockedNumber>, IBlockedNumberRepository
    {
        private ApplicationDbContext _db;
        public BlockedNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
