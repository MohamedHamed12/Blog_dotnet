using System.Linq.Expressions;

namespace BlogBackend.Core.Specifications
{
    public class BaseSpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
    }
}
