using BlogBackend.API.Filters;

namespace BlogBackend.API.DTOs
{
    public class PostFilterDTO
    {
        public StringFilter? Title { get; set; }
        public StringFilter? Content { get; set; }
        public NumberFilter<int>? UserId { get; set; }
        public NumberFilter<DateTime>? PublishedAt { get; set; }
    }
}
