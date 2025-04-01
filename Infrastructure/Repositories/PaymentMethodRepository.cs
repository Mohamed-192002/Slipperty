namespace Infrastructure.Repositories
{
    public class PaymentMethodRepository : BaseRepository<PaymentMethod>, IPaymentMethodRepository
    {
        private ApplicationDbContext _db;
        public PaymentMethodRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
