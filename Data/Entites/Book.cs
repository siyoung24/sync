namespace MemoApp.Data.Entities;

public class Book
{
    public int Id { get; set; } // 기본 키
    public string Title { get; set; } = string.Empty; // 책 제목
    public string Author { get; set; } = string.Empty; // 책 저자
    public int TotalPages { get; set; } // 총 페이지 수
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 등록된 날짜
}
