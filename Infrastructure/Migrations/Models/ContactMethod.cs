namespace Infrastructure.Models;

public class ContactMethod
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int ContactMethodId { get; set; }
	[MaxLength(100)]
	[Required(ErrorMessage = "يجب ادخل اسم طريقة التواصل")]
	public string ContactMethodName { get; set; } = null!;
}