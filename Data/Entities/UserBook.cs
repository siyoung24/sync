namespace MemoApp.Data.Entities;

public class UserBook
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public int CurrentPage { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}
