namespace Contracts.Specification;

public class OrSpecification<TInput> : ISpecification<TInput>
{
    private readonly ISpecification<TInput> _left;
    private readonly ISpecification<TInput> _right;

    public OrSpecification(ISpecification<TInput> left, ISpecification<TInput> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        _left = left;

        ArgumentNullException.ThrowIfNull(right);
        _right = right;
    }

    public bool IsSatisfiedBy(TInput input) => _left.IsSatisfiedBy(input) || _right.IsSatisfiedBy(input);
}
