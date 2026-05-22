namespace MemoApp.Books.Dtos;

public class MyBookDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int TotalPages { get; set; }
    public string? Cover { get; set; }
    public string? Publisher { get; set; }
    public string? Isbn13 { get; set; }
    public int CurrentPage { get; set; }
    public DateTime AddedAt { get; set; }
}
