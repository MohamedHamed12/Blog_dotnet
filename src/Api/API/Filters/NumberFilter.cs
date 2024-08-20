namespace BlogBackend.API.Filters
{
    public class NumberFilter<T> where T : struct, IComparable
    {
        public T? Equal { get; set; }
        public T? LessThan { get; set; }
        public T? GreaterThan { get; set; }
    }
}
