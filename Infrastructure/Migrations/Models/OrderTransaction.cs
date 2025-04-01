namespace Infrastructure.Models;

public class OrderTransaction : IModelBase
{
	[Key]
	[Column(Order = 0)]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public long TransactionID { get; set; }
	[Column(Order = 1)]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int OrderID { get; set; } 
	public int 	OperationType { get; set; }
	public int 	PerformedBy { get; set; }
	public string 	Details { get; set; } = string.Empty;
	public DateTime	CreatedAt {get; set;}  
	[ForeignKey("OrderID")]
	public  OrderHead OrderHead { get; set; } = null!;
	public DateTime? CreatedOn { get; set; }
	public DateTime? ModifiedOn { get; set; }
	public string? CreatedBy { get; set; }
	public string? ModifiedBy { get; set; }
}

// ENUM('تحضير', 'تأكيد', 'متابعة', 'إلغاء', 'تأجيل', 'شحن') NOT NULL,