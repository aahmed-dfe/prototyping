namespace Contracts.Specification;

public sealed class NotSpecification<TInput> : ISpecification<TInput>
{
    private readonly ISpecification<TInput> _input;

    public NotSpecification(ISpecification<TInput> input)
    {
        ArgumentNullException.ThrowIfNull(input);
        _input = input;
    }

    public bool IsSatisfiedBy(TInput input) => !_input.IsSatisfiedBy(input);
}
