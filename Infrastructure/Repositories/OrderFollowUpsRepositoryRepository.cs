namespace Infrastructure.Repositories;

public class OrderFollowUpsRepositoryRepository : BaseRepository<OrderFollowUps> , IOrderFollowUpsRepository
{
	private readonly ApplicationDbContext _db;

	public OrderFollowUpsRepositoryRepository(ApplicationDbContext db) : base(db)
	{
		_db = db;
	}
}