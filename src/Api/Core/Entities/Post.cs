using Sieve.Attributes;

public class Post
{


    [Sieve(CanFilter = true, CanSort = true)]
    public Guid Id { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]

    public string Title { get; set; } = string.Empty;
    [Sieve(CanFilter = true, CanSort = true)]

    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
