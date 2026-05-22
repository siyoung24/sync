namespace MemoApp.Reviews.Dtos;

// 내가 쓴 감상평 - 책 정보 같이 노출
public class MyReviewDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string? Cover { get; set; }
    public int Page { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
