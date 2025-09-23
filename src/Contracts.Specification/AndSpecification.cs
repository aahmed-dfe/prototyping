namespace Contracts.Specification;

public class AndSpecification<TInput> : ISpecification<TInput>
{
    private readonly ISpecification<TInput> _left;
    private readonly ISpecification<TInput> _right;

    public AndSpecification(ISpecification<TInput> left, ISpecification<TInput> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        _left = left;

        ArgumentNullException.ThrowIfNull(right);
        _right = right;
    }

    public bool IsSatisfiedBy(TInput input) => _left.IsSatisfiedBy(input) && _right.IsSatisfiedBy(input);
}
