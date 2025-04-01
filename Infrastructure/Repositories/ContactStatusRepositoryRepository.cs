namespace Infrastructure.Repositories;

public class ContactStatusRepositoryRepository : BaseRepository<ContactStatus> , IContactStatusRepository
{
	private readonly ApplicationDbContext _db;
	public ContactStatusRepositoryRepository(ApplicationDbContext db) : base(db)
	{
		_db = db;
	}
	
}