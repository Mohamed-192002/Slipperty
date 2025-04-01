namespace Infrastructure.Models;

public class OperationType
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int OperationTypeId { get; set; }
	public string OperationTypeName { get; set; } = null!;
}

public enum ActionType
{  
	New = 1,
	Viewed ,
	Holding, 
	Cancel, 
	FollowUps, 
	Modified, 
	Confirmed,
	Prepared,
	Shipped,
	Delivered,
    Urgent,
    ModifiedProduct , 
    NoAttamptYeasterday , 
    StoppedConfirmed, 
    DeleteOrderDetail,
    CloseModal,
	ModificationDeclined
    
}




