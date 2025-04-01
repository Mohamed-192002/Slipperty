namespace Infrastructure.Contracts;

public interface IModelBase
{
	DateTime? CreatedOn { get; set; }
	DateTime? ModifiedOn { get; set; }
	string? CreatedBy { get; set; }
	string? ModifiedBy { get; set; }
	
}