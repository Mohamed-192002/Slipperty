namespace Business.Helpers;

public class ContactStatusHelper
{

	public static Dictionary<int, string> ContactStatus = new Dictionary<int, string>()
	{
		{
			1,
			"لا يرد"
		},
		{
			2,
			"مغلق"
		},
		{
			3,
			"مش بيجمع"
		},
		{
			4,
			"رد و قفل"
		},
		{
			5 , 
			"اخري"
		}
	};

	public static string GetContactStatus(int contactId)
	{
		string contactStatus = "اخري";
		if (ContactStatus.TryGetValue(contactId, out contactStatus))
		{
			return contactStatus;
		}
		
		return contactStatus;
	}

}