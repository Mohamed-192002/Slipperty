namespace Infrastructure.Repositories
{
    public class VariableCombinationRepository : BaseRepository<VariableCombination>, IVariableCombinationRepository
    {
        private ApplicationDbContext _db;
        public VariableCombinationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
