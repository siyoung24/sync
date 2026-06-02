namespace MemoApp.UserBooks.Dtos;

public class CreateUserBookDto
{
    public int UserId { get; set; }   // 책장 주인 (요청에서 전달)
    public int BookId { get; set; }   // 담을 책
}
