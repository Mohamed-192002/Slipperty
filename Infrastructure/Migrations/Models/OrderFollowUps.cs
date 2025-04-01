namespace Infrastructure.Models;

public class OrderFollowUps : IModelBase
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long FollowUpID { get; set; }
	public int OrderID { get; set; }
	//public int 	PerformedBy { get; set; }
	public int? ContactMethodId { get; set; }
	public int? ContactStatusId {get; set;}
	[Column(TypeName = "text")]
	public string?	Notes  { get; set; }
	
	public int UserID { get; set; }  
	[ForeignKey("ContactMethodId")]
	public virtual ContactMethod ContactMethod { get; set; }
	[ForeignKey("ContactStatusId")]
	public virtual ContactStatus ContactStatus { get; set; }
	
	[ForeignKey("OrderID")]
	public virtual OrderHead OrderHead { get; set; }
	public DateTime? CreatedOn { get; set; }
	public DateTime? ModifiedOn { get; set; }
	public string? CreatedBy { get; set; }
	public string? ModifiedBy { get; set; }
}


public class OrderFollowUpsDto
{
	public string?	Notes  { get; set; }

}

