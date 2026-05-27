namespace Sync.WinForms.Models;

public class BookSearchResult
{
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string? Publisher { get; set; }
    public string? Isbn13 { get; set; }
    public string? Cover { get; set; }
}

public class AddMyBookRequest
{
    public string Isbn13 { get; set; } = "";
}

public class MyBook
{
    public int BookId { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public int TotalPages { get; set; }
    public string? Cover { get; set; }
    public string? Publisher { get; set; }
    public string? Isbn13 { get; set; }
    public int CurrentPage { get; set; }
    public DateTime AddedAt { get; set; }
}

public class UpdatePageRequest
{
    public int CurrentPage { get; set; }
}