namespace Infrastructure.Repositories
{
    public class UserPaymentMethodRepository : BaseRepository<UserPaymentMethod>, IUserPaymentMethodRepository
    {
        private ApplicationDbContext _db;
        public UserPaymentMethodRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
