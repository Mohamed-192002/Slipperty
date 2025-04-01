namespace Infrastructure.Repositories;

public class ContactMethodRepositoryRepository : BaseRepository<ContactMethod> , IContactMethodRepository
{
	private readonly ApplicationDbContext _dbContext;
	public ContactMethodRepositoryRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
	
}