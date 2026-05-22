namespace MemoApp.Data.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int TotalPages { get; set; }
    public string? Isbn13 { get; set; }
    public string? Publisher { get; set; }
    public string? Cover { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
