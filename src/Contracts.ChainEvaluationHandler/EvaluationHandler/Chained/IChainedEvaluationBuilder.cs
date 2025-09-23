using Contracts.ChainEvaluationHandler.EvaluationHandler.Abstractions;

namespace Contracts.ChainEvaluationHandler.EvaluationHandler.Chained;

public interface IChainedEvaluationBuilder<TIn, TOut>
{
    IChainedEvaluationBuilder<TIn, TOut> ChainNext(IEvaluationHandler<TIn, TOut> handler);
    IEvaluationHandler<TIn, TOut> Build();
}
