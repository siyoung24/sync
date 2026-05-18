namespace MemoApp.Reviews.Dtos;

// 한줄평 조회 입력 - 어떤 책의, 현재 몇 페이지까지 읽었는지
public class ReviewQueryDto
{
    public int BookId { get; set; }
    public int CurrentPage { get; set; }
}
