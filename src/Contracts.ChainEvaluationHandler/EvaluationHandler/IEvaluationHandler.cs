namespace Contracts.ChainEvaluationHandler.EvaluationHandler;
public interface IEvaluationHandler<in TInput, out TOutput>
{
    bool CanEvaluate(TInput input);
    TOutput Evaluate(TInput input);
}
