using BlogBackend.API.DTOs;
using BlogBackend.API.Filters;
using Core.Entities;
// using BlogBackend.Core.Entities;

namespace BlogBackend.Core.Specifications
{
    public class PostSpecification : BaseSpecification<Post>
    {
        public PostSpecification(PostFilterDTO filter)
            : base(p =>
                (filter.Title == null || ApplyStringFilter(p.Title, filter.Title)) &&
                (filter.Content == null || ApplyStringFilter(p.Content, filter.Content)) )
                // (filter.UserId == null || ApplyNumberFilter(p.UserId, filter.UserId)) &&
                // (filter.PublishedAt == null || ApplyNumberFilter(p.PublishedAt, filter.PublishedAt)))
        {
        }

        private static bool ApplyStringFilter(string value, StringFilter filter)
        {
            return
                (string.IsNullOrEmpty(filter.Contains) || value.Contains(filter.Contains)) &&
                (string.IsNullOrEmpty(filter.StartsWith) || value.StartsWith(filter.StartsWith)) &&
                (string.IsNullOrEmpty(filter.Exact) || value == filter.Exact);
        }

        private static bool ApplyNumberFilter<T>(T value, NumberFilter<T> filter) where T : struct, IComparable
        {
            return
                (!filter.Equal.HasValue || value.CompareTo(filter.Equal.Value) == 0) &&
                (!filter.LessThan.HasValue || value.CompareTo(filter.LessThan.Value) < 0) &&
                (!filter.GreaterThan.HasValue || value.CompareTo(filter.GreaterThan.Value) > 0);
        }
    }
}
