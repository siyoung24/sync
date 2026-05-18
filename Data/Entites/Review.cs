namespace MemoApp.Data.Entities;

public class Review
{
    public int Id { get; set; }                           // 기본 키
    public int BookId { get; set; }                       // 어떤 책에 대한 한줄평인지 (Book FK)
    public int WriterId { get; set; }                     // 작성자 (User FK)
    public int Page { get; set; }                         // 한줄평이 작성된 페이지
    public string Content { get; set; } = string.Empty;   // 한줄평 내용
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 작성 시각
}
