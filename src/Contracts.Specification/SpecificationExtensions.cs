namespace Contracts.Specification;

public static class SpecificationExtensions
{
    public static ISpecification<TInput> And<TInput>(
        this ISpecification<TInput> left,
        ISpecification<TInput> right) =>
        new AndSpecification<TInput>(left, right);

    public static ISpecification<TInput> Or<TInput>(
        this ISpecification<TInput> left,
        ISpecification<TInput> right) =>
        new OrSpecification<TInput>(left, right);

    public static ISpecification<TInput> Not<TInput>(
        this ISpecification<TInput> spec) =>
        new NotSpecification<TInput>(spec);
}
