namespace Contracts.Specification;

public interface ISpecification<in TInput>
{
    public bool IsSatisfiedBy(TInput input);
}
