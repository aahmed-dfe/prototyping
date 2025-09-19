// See https://aka.ms/new-console-template for more information

using Contracts.ChainEvaluationHandler.EvaluationHandler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// Register Handlers
builder.Services.AddSingleton<HandlerA>();
builder.Services.AddSingleton<HandlerB>();

// Build Chained handler and register root handler
builder.Services.AddSingleton<IEvaluationHandler<string, string>>(sp =>
{
    ChainEvaluationHandler<string, string> handlerA = new(sp.GetRequiredService<HandlerA>());

    ChainEvaluationHandler<string, string> handlerB = new(sp.GetRequiredService<HandlerB>());

    return ChainedEvaluationHandlerBuilder<string, string>
        .Create(handlerA)
        .ChainNext(handlerB)
        .Build();
});

using IHost host = builder.Build();

// Resolve and use the chained handler
IEvaluationHandler<string, string> chainedHandler
    = host.Services.GetRequiredService<IEvaluationHandler<string, string>>();

string result = chainedHandler.Evaluate("some input");

Console.WriteLine($"Evaluation result: {result}");

public sealed class HandlerA : IEvaluationHandler<string, string>
{
    public bool CanEvaluate(string input) => false;
    public string Evaluate(string input) => "HandleA";
}

public sealed class HandlerB : IEvaluationHandler<string, string>
{
    public bool CanEvaluate(string input) => true;
    public string Evaluate(string input) => "HandleB";
}
