namespace Infrastructure.Repositories
{
    public class VariableValueRepository : BaseRepository<VariableValue>, IVariableValueRepository
    {
        private ApplicationDbContext _db;
        public VariableValueRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
