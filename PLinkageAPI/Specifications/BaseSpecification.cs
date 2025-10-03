using PLinkageAPI.Interfaces;
using System.Linq.Expressions;

namespace PLinkageAPI.Specifications
{
    // BaseSpecification<T>.cs (Base class for easy construction)
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; }
    }
}
