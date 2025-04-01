namespace Infrastructure.Models;

public class ContactStatus
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int ContactStatusId { get; set; }
	[MaxLength(500)]
	[Required(ErrorMessage = "يجب كتابت اسم الحالة")]
	public string ContactStatusName { get; set; } = null!;
}