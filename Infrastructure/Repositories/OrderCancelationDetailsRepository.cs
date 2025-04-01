using Infrastructure.Migrations.Models;

namespace Infrastructure.Repositories;

public class OrderCancelationDetailsRepository :  BaseRepository<OrderCancelationDetails> , IOrderCancelationDetailsRepository
{
	private readonly ApplicationDbContext _db;
	public OrderCancelationDetailsRepository(ApplicationDbContext db) : base(db)
	{
		_db = db;
	}
	
}