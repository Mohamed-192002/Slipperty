namespace Infrastructure.Repositories;

public class OperationTypeRepositoryRepository : BaseRepository<OperationType> , IOperationTypeRepository
{
	private readonly ApplicationDbContext _db;
	public OperationTypeRepositoryRepository(ApplicationDbContext db) : base(db)
	{
		_db = db;
	}
}