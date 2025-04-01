namespace Infrastructure.Repositories
{
    public class MaterialRepository : BaseRepository<Material>, IMaterialRepository
    {
        private ApplicationDbContext _db;
        public MaterialRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
