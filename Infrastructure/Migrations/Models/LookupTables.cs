namespace Infrastructure.Migrations.Models;

public static class LookupTables
{
	public static Dictionary<string, Type> LookUpsTable = new Dictionary<string, Type>()
	{
		{ "AddressType", typeof(AddressType) },
		{"Region", typeof(Region) },
	}; 
}