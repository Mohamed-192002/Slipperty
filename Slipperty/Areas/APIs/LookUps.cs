using Business.Shard;
using Infrastructure.Contracts;
using Infrastructure.Contracts.Seeds;

namespace Slipperty.Areas.APIs;

[Authorize(Roles = Roles.Role_Admin)]
[Route("api/[controller]")]
public class LookUps : Controller
{
	//private Dictionary<string , IBaseRepository<T>> LookUps = new Dictionary<string, IBaseRepository<T>>(); 


	private readonly IGovernmentsBusiness governmentsBusiness ;
	private readonly IRegionsBusiness regionsBusiness; 
	public LookUps(IRegionsBusiness regionsBusiness, IGovernmentsBusiness governmentsBusiness)
	{
		this.regionsBusiness = regionsBusiness;
		this.governmentsBusiness = governmentsBusiness;
	}

	
	[HttpGet("GetGovernments")]
	public IActionResult GetGovernments()
	{
		return Ok(this.governmentsBusiness.GetAll());
	}
	
	
	[HttpGet("GetRegion/{GovernmentId}")]
	public IActionResult GetRegion(int? GovernmentId)
	{
		return Ok(regionsBusiness.GetAll().Where(_=> _.GovernmentId == GovernmentId));
	}
	
	
	// under implementation  
	
	// [HttpPost("GetLookUps/{LookUpName}")]
	// public IActionResult GetLookUps(Page page , string? LookUpName = null!)
	// {
	// 	
	// 	
	// 	return Ok();
	// }	
}



