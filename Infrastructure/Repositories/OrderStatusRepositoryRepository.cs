namespace Infrastructure.Repositories;

public class OrderStatusRepositoryRepository : BaseRepository<OrderStatus> , IOrderStatusRepository
{
	private readonly ApplicationDbContext _db;

	public OrderStatusRepositoryRepository(ApplicationDbContext db) : base(db)
	{
		_db = db;
	}
}