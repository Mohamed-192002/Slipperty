namespace Infrastructure.Repositories;

public class OrderTransactionRepositoryRepository : BaseRepository<OrderTransaction> , IOrderTransactionRepository
{
	 private readonly ApplicationDbContext _db;

	 public OrderTransactionRepositoryRepository(ApplicationDbContext db) : base(db)
	 {
		 _db = db;
	 }
}