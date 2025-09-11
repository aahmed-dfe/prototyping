namespace Contracts.EvaluationHandler;
public class ChainEvaluationHandler<TInput, TOutput> : IChainedEvaluationHandler<TInput, TOutput>
{
    private readonly IEvaluationHandler<TInput, TOutput> _current;
    private IEvaluationHandler<TInput, TOutput>? _next;

    public ChainEvaluationHandler(IEvaluationHandler<TInput, TOutput> current)
    {
        _current = current ??
            RootEvaluationHandler.Create();
    }

    public virtual bool CanEvaluate(TInput input) => true;
    public virtual TOutput Evaluate(TInput input)
    {
        if (_current.CanEvaluate(input))
        {
            return _current.Evaluate(input);
        }

        IEvaluationHandler<TInput, TOutput> evaluationHandler = _next ??= RootEvaluationHandler.Create();
        return evaluationHandler.Evaluate(input);
    }

    public IEvaluationHandler<TInput, TOutput> ChainNext(IEvaluationHandler<TInput, TOutput> handler)
    {
        _next =
            handler is null ? RootEvaluationHandler.Create() :
                handler;

        return this;
    }

    // TODO adjust the behaviour of RootEvaluationHandler where the Root is reached - factory, which creates based on options
    private sealed class RootEvaluationHandler : IEvaluationHandler<TInput, TOutput>
    {
        private RootEvaluationHandler() { }
        public static IEvaluationHandler<TInput, TOutput> Create() => new RootEvaluationHandler();
        public bool CanEvaluate(TInput input) => true;
        public TOutput Evaluate(TInput input) => throw new InvalidOperationException("End of chain reached without evaluation");
    }
}
