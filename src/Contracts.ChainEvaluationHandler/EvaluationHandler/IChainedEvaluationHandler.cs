namespace Contracts.ChainEvaluationHandler.EvaluationHandler;
public interface IChainedEvaluationHandler<TInput, TOutput> : IEvaluationHandler<TInput, TOutput>
{
    public IEvaluationHandler<TInput, TOutput> ChainNext(IEvaluationHandler<TInput, TOutput> handler);
}
