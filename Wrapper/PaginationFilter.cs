namespace fundit_server.Wrapper;

public class PaginationFilter 
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
    
    public string? SearchValue { get; set; }
    public bool UsePaging { get; set; } = true;

    public string[]? OrderBy { get; set; }
}
