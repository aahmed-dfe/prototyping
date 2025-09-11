namespace Contracts.EvaluationHandler;
internal sealed class ExampleChainEvaluationHandler : ChainEvaluationHandler<string, string>
{
    public ExampleChainEvaluationHandler(IEvaluationHandler<string, string> current) : base(current)
    {
    }

    public override bool CanEvaluate(string input) => !string.IsNullOrEmpty(input);
    public override string Evaluate(string input) => string.Empty;
}
