namespace MemoApp.Books.Dtos;

public class CreateBookDto
{
    public string Title { get; set; } = string.Empty;   // 책 제목 (필수)
    public string Author { get; set; } = string.Empty;  // 저자 이름 (필수)
    public int TotalPages { get; set; }                  // 전체 페이지 수 (유효성 검사 기준)
}
    