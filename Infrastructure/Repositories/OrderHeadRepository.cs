namespace Infrastructure.Repositories
{
    public class OrderHeadRepository : BaseRepository<OrderHead>, IOrderHeadRepository
    {
        private ApplicationDbContext _db;
        public OrderHeadRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
