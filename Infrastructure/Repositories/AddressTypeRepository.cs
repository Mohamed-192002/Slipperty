namespace Infrastructure.Repositories
{
    public class AddressTypeRepository : BaseRepository<AddressType>, IAddressTypeRepository
    {
        private ApplicationDbContext _db;
        public AddressTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
