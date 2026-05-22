namespace MemoApp.Books.Dtos;

public class BookQueryDto
{
    public string? SearchKeyword { get; set; }           // 검색어 (없으면 전체 조회, null 허용)
    public string SortBy { get; set; } = "latest";      // 정렬 기준: latest-최신순
}
