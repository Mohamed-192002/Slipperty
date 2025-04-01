namespace Infrastructure.Models;

public class OrderStatus
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int StatusId { get; set; }
	public string StatusName { get; set; } = string.Empty;
	public IEnumerable<OrderHead> OrderHeads { get; set; } = new List<OrderHead>();
}