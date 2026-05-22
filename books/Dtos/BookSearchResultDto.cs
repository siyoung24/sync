namespace MemoApp.Books.Dtos;

// 외부 API(알라딘) 검색 결과 - 아직 우리 DB에 등록 안 된 책
public class BookSearchResultDto
{
    public string Title { get; set; } = string.Empty;       // 책 제목
    public string Author { get; set; } = string.Empty;      // 저자
    public string Publisher { get; set; } = string.Empty;   // 출판사
    public int TotalPages { get; set; }                     // 페이지 수 (한줄평 페이지 매칭용)
    public string Isbn13 { get; set; } = string.Empty;      // ISBN-13 (중복 체크 키로 사용 가능)
    public string Cover { get; set; } = string.Empty;       // 표지 이미지 URL
}
