namespace MemoApp.UserBooks.Dtos;

public class UserBookDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateTime CreatedAt { get; set; }
}
