using BlogBackend.API.Filters;

namespace BlogBackend.API.DTOs
{
    public class UserFilterDTO
    {
        public StringFilter? Username { get; set; }
        public StringFilter? Email { get; set; }
        public NumberFilter<DateTime>? CreatedAt { get; set; }
    }
}
