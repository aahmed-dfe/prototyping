using Contracts.ChainEvaluationHandler.EvaluationHandler.Abstractions;

namespace Contracts.ChainEvaluationHandler.EvaluationHandler.Chained;

public sealed class ChainedEvaluationHandlerBuilder<TInput, TOutput> : IChainedEvaluationBuilder<TInput, TOutput>
{
    private readonly List<IChainedEvaluationHandler<TInput, TOutput>> _handlers;

    private ChainedEvaluationHandlerBuilder() => _handlers = [];

    private IChainedEvaluationHandler<TInput, TOutput> Head => _handlers[_handlers.Count - 1];

    public IChainedEvaluationBuilder<TInput, TOutput> ChainNext(IEvaluationHandler<TInput, TOutput> next)
    {
        ArgumentNullException.ThrowIfNull(next);
        IChainedEvaluationHandler<TInput, TOutput> chainedHandler = next.ToChainedHandler();
        Head.ChainNext(chainedHandler);
        _handlers.Add(chainedHandler);
        return this;
    }

    public IEvaluationHandler<TInput, TOutput> Build()
    {
        if (_handlers.Count == 0)
        {
            throw new ArgumentException("No handlers chained");
        }

        return _handlers[0];
    }

    public static ChainedEvaluationHandlerBuilder<TInput, TOutput> Create() => new();
}
