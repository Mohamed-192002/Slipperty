namespace Business.Shard;

public class Page
{
	public Page()
	{
	  PageNumber = 1;
	  PageSize = 10;
	  Filter = ""; 
	}
	public int PageNumber { get; set; }
	public int PageSize { get; set; }
	public string? Filter { get; set; }
	
}