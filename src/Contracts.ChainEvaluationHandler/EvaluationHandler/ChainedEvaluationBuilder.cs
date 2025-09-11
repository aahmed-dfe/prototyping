namespace Contracts.ChainEvaluationHandler.EvaluationHandler;
public sealed class ChainedEvaluationHandlerBuilder<TInput, TOutput>
{
    private readonly List<IChainedEvaluationHandler<TInput, TOutput>> _handlers;

    private ChainedEvaluationHandlerBuilder(IChainedEvaluationHandler<TInput, TOutput> current)
    {
        ArgumentNullException.ThrowIfNull(current);
        _handlers = [current];
    }
    private IChainedEvaluationHandler<TInput, TOutput> Head => _handlers[_handlers.Count - 1];

    public static ChainedEvaluationHandlerBuilder<TInput, TOutput> Create(IChainedEvaluationHandler<TInput, TOutput> head) => new(head);

    public ChainedEvaluationHandlerBuilder<TInput, TOutput> ChainNext(IChainedEvaluationHandler<TInput, TOutput> next)
    {
        ArgumentNullException.ThrowIfNull(next);
        Head.ChainNext(next);
        _handlers.Add(next);
        return this;
    }

    public IChainedEvaluationHandler<TInput, TOutput> Build()
    {
        if (_handlers.Count == 0)
        {
            throw new ArgumentException("No handlers chained");
        }

        return _handlers[0];
    }
}
