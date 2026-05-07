namespace MemoApp.Books.Dtos;

public class BookDto
{
    public int Id { get; set; }                          // 책 고유 ID
    public string Title { get; set; } = string.Empty;   // 책 제목
    public string Author { get; set; } = string.Empty;  // 저자 이름
    public int TotalPages { get; set; }                  // 전체 페이지 수
    public DateTime CreatedAt { get; set; }              // 등록 날짜 (응답 시 포함)
}
