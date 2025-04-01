namespace Infrastructure.Repositories
{
    public class GovernmentRepository : BaseRepository<Government>, IGovernmentRepository
    {
        private ApplicationDbContext _db;
        public GovernmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
