using System.Linq.Expressions;

namespace PLinkageAPI.Interfaces
{
    // ISpecification<T>.cs (Core Layer)
    public interface ISpecification<T>
    {
        // The C# Expression used by the service layer
        Expression<Func<T, bool>> Criteria { get; }

        // Optional: Includes (less common/handled differently in NoSQL)
        // Optional: Ordering (handled by the MongoDB repository)
    }
}
