public class Comment
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid PostId { get; set; }
    public Post Post { get; set; }
    public string UserId { get; set; }
}
