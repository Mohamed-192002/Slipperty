using System.Linq.Expressions;

namespace Business.Helpers.Extensions;

public  static class IQueryableExtensions
{
	public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, Dictionary<string, object> filters)
	{
		if (filters == null || !filters.Any())
			return query;

		var parameter = Expression.Parameter(typeof(T), "x");
		var expressions = new List<Expression>();

		foreach (var filter in filters)
		{
			
			var property = typeof(T).GetProperty(filter.Key);
			if (property == null) continue; 

			var filterValue = filter.Value;
			if (filterValue == null) continue; 

			
			if (filterValue is string str && string.IsNullOrWhiteSpace(str)) 
				continue; 

			
			var propertyAccess = Expression.Property(parameter, property);
			var valueExpression = Expression.Constant(filterValue, property.PropertyType);

			var comparison = Expression.Equal(propertyAccess, valueExpression);
			expressions.Add(comparison);
		}

		if (!expressions.Any()) return query;
		
		var finalExpression = expressions.Aggregate(Expression.AndAlso);
		var lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);

		return query.Where(lambda);
	}
	
	public static IQueryable<OrderHeadDTO> ApplyFilters(this IQueryable<OrderHeadDTO> query, OrderCardFilterDto filters)
	{
		if (!string.IsNullOrEmpty(filters.CustomerName))
			query = query.Where(o => o.fullName.Contains(filters.CustomerName));

		if (!string.IsNullOrEmpty(filters.PhoneNumber))
			query = query.Where(o => o.phoneNumber.Contains(filters.PhoneNumber) || o.phoneNumber2.Contains(filters.PhoneNumber));

		if (!string.IsNullOrEmpty(filters.GovernmentId.ToString()))
			query = query.Where(o => o.governmentId == filters.GovernmentId);

		if (!string.IsNullOrEmpty(filters.RegionId.ToString()))
			query = query.Where(o => o.regionId == filters.RegionId);

		if (!string.IsNullOrEmpty(filters.Address))
			query = query.Where(o => o.Address.Contains(filters.Address));

		if (filters.InsertDateFrom.HasValue)
			query = query.Where(o => o.RegDate >= filters.InsertDateFrom.Value);

		if (filters.InsertDateTo.HasValue)
			query = query.Where(o => o.RegDate <= filters.InsertDateTo.Value);
		
		return query;
	}

	public static IQueryable<OrderHeadDTO> ApplySort(this IQueryable<OrderHeadDTO> query, string SortType)
	{
		return SortType.ToUpper() switch
		{
			"ASC" => query.OrderBy(_ => _.RegDate),
			"DESC" => query.OrderByDescending(_ => _.RegDate),
			_ => query
		};
	}
}