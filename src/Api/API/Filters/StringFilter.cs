namespace BlogBackend.API.Filters
{
    public class StringFilter
    {
        public string? Contains { get; set; }
        public string? StartsWith { get; set; }
        public string? Exact { get; set; }
    }
}
