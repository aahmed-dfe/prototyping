using Contracts.ChainEvaluationHandler.EvaluationHandler.Abstractions;

namespace Contracts.ChainEvaluationHandler.EvaluationHandler.Chained;

public interface IChainedEvaluationHandler<TInput, TOutput> : IEvaluationHandler<TInput, TOutput>
{
    public IChainedEvaluationHandler<TInput, TOutput> ChainNext(IEvaluationHandler<TInput, TOutput> handler);
}
