namespace Sync.WinForms.Models;

public class CreateReviewRequest
{
    public int BookId { get; set; }
    public int Page { get; set; }
    public string Content { get; set; } = "";
}

public class Review
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int WriterId { get; set; }
    public int Page { get; set; }
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class MyReview
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = "";
    public string? Cover { get; set; }
    public int Page { get; set; }
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}