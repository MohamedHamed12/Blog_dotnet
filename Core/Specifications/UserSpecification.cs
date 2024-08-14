using BlogBackend.API.DTOs;
using BlogBackend.API.Filters;
using Core.Entities;

namespace BlogBackend.Core.Specifications
{
    public class UserSpecification : BaseSpecification<User>
    {
        public UserSpecification(UserFilterDTO filter)
            : base(u =>
                (filter.Username == null || ApplyStringFilter(u.Username, filter.Username)) &&
                (filter.Email == null || ApplyStringFilter(u.Email, filter.Email)))  

               //  (filter.CreatedAt == null || ApplyNumberFilter(u.CreatedAt, filter.CreatedAt))
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
