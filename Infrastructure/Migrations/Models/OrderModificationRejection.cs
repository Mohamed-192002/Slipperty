namespace Infrastructure.Migrations.Models;

public class OrderModificationDeclined  : IModelBase
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	
	public int OrderId { get; set; }
	
	public string? Reason { get; set; } = string.Empty;
	[ForeignKey("OrderId")]
	public OrderHead OrderHead { get; set; } = null!;
	public DateTime? CreatedOn { get; set; }
	public DateTime? ModifiedOn { get; set; }
	public string? CreatedBy { get; set; }
	public string? ModifiedBy { get; set; }
}