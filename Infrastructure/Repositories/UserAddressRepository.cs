namespace Infrastructure.Repositories
{
    public class UserAddressRepository : BaseRepository<UserAddress>, IUserAddressRepository
    {
        private ApplicationDbContext _db;
        public UserAddressRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
