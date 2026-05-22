namespace MemoApp.Reviews.Dtos;

public class CreateReviewDto
{
    public int BookId { get; set; }
    public int Page { get; set; }
    public string Content { get; set; } = string.Empty;
}
