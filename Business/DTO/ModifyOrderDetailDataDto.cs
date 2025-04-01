using System.Text.Json.Serialization;

namespace Business.DTO;

public class ModifyOrderDetailDataDto
{
	public string FullName { get; set; } = string.Empty; 
	public string PhoneNumber { get; set; } = string.Empty;
	public string? PhoneNumber2 { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public int GovernmentId { get; set; }
	public int RegionId { get; set; }
	public string? Notes { get; set; }
	public string? Rotes { get; set; }
	public string? DeliveryTimeFrom { get; set; }
	public string? DeliveryTimeTo { get; set; }
}