namespace Business.DTO
{
    public class PaginationDTO
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int DisplayedItemsCount { get; set; }
        public bool IsPreviousLinkVisible { get; set; }
        public bool IsNextLinkVisible { get; set; }
   
        public object? Filter {get;set;}
        public string? Action { get; set; }
        public string? Controller { get; set; }  
    }
}
