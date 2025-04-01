namespace Business.DTO;

public class ModifyOrderRequestDto
{
	public string?  OrderUserAddress { get; set; }
	public string? OrderUserPhone { get; set; }
	public IEnumerable<ModifyOrderDetailsRequestDto>? ModifyOrderOrderDetailsRequest { get; set; } = Enumerable.Empty<ModifyOrderDetailsRequestDto>();
}