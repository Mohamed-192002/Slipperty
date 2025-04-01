namespace Business.DTO;

public class OrderCardFilterDto
{
	public string? CustomerName { get; set; } = null;
	public string? PhoneNumber { get; set; } = null;
	public int? GovernmentId { get; set; } = null;
	public int? RegionId { get; set; } 
	public string? Address { get; set; } = null;
	public DateTime? InsertDateFrom { get; set; } = null;
	public DateTime? InsertDateTo { get; set; } = null;
	public string? OrderNo  { get; set; } = null;
	public bool IsshowSearchSection { get; set; } = false; 
	public string? SortType { get; set; } = "DESC";
	public int? HoursSinceLastTransaction  { get; set; } = null;
}