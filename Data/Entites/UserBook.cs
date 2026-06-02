namespace MemoApp.Data.Entities;

public class UserBook
{
    public int Id { get; set; }                                   // 기본 키
    public int UserId { get; set; }                               // 책장 주인 (User FK)
    public int BookId { get; set; }                               // 담긴 책 (Book FK)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;    // 책장에 담은 시각
}
