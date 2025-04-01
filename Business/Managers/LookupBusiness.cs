using Business.Shard;
using Infrastructure.Migrations.Models;
using Microsoft.AspNetCore.Mvc;

namespace Business.Managers;


public interface ILookupBusiness
{
	public object GetLookupByID(object id);
	public OkObjectResult GetLookupsData(string? lookUpName = "");
	public OkObjectResult GetLookupsData(string? lookUpName = null, Page page = null ); 
	
	
}


public class LookupBusiness : ILookupBusiness
{
   private IUnitOfWork _unitOfWork;
	public LookupBusiness(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}
	public object GetLookupByID(object id)
	{
		throw new NotImplementedException();
	}

	public OkObjectResult GetLookupsData(string? lookUpName = "")
	{
		if(string.IsNullOrWhiteSpace(lookUpName)) throw new Exception("Invalid lookup name");

		var repository =  _unitOfWork.GetLookUpRepository(lookUpName);
		var method = repository.GetType().GetMethod("GetAll");
		var result = method.Invoke(repository, null);
		return new OkObjectResult(result); 

	}

	public OkObjectResult GetLookupsData(string? lookUpName = null, Page page = null)
	{
		return GetLookupsData(lookUpName); 
	}
}