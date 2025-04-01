using System.Reflection;

namespace Business.Helpers.Extensions;

public static class ObjectExtensions
{
	public static Dictionary<string, object> ToFilterDictionary(this object obj)
	{
		if (obj == null)
			return new Dictionary<string, object>();

		return obj.GetType()
			.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(p => p.GetValue(obj) != null) 
			.ToDictionary(p => p.Name, p => p.GetValue(obj)!);
	}
}