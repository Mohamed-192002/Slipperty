using Infrastructure.Migrations.Models;

namespace Infrastructure.Repositories;

public class OrderModificationDeclinedRepository : BaseRepository<OrderModificationDeclined>, IOrderModificationDeclinedRepository
{
	public OrderModificationDeclinedRepository(ApplicationDbContext db) : base(db)
	{
	}
}