namespace MemoApp.Reviews.Dtos;

public class ReviewDto
{
    public int Id { get; set; }                          // 한줄평 고유 ID
    public int BookId { get; set; }                      // 책 ID
    public int WriterId { get; set; }                    // 작성자 ID
    public int Page { get; set; }                        // 작성 페이지
    public string Content { get; set; } = string.Empty;  // 한줄평 내용
    public DateTime CreatedAt { get; set; }              // 작성 시각
}
