using Contracts.ChainEvaluationHandler.EvaluationHandler.Abstractions;
using Contracts.ChainEvaluationHandler.EvaluationHandler.Chained;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts.ChainEvaluationHandler;

public static class CompositionRoot
{
    public static IServiceCollection V1AddStatelessChainedEvaluationHandlers<TIn, TOut>(
        this IServiceCollection services,
        List<Func<IServiceProvider, IEvaluationHandler<TIn, TOut>>> resolveableHandlerChain)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.AddSingleton<IEvaluationHandler<TIn, TOut>>(sp =>
        {
            ChainedEvaluationHandlerBuilder<TIn, TOut> builder =
                ChainedEvaluationHandlerBuilder<TIn, TOut>.Create(
                    resolveableHandlerChain[0]
                        .Invoke(sp).ToChainedHandler());

            foreach (Func<IServiceProvider, IEvaluationHandler<TIn, TOut>> resolveHandler in resolveableHandlerChain
                         .Skip(1))
            {
                builder.ChainNext(
                    resolveHandler.Invoke(sp)
                        .ToChainedHandler());
            }

            return builder.Build();
        });
        return services;
    }

    // Resolve from SP
    public static IServiceCollection V2AddStatelessChainedEvaluationHandlers<TIn, TOut>(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);


        services.AddSingleton<IEvaluationHandler<TIn, TOut>>(sp =>
        {
            List<IEvaluationHandler<TIn, TOut>> evaluationHandlers =
                (sp.GetServices<IEvaluationHandler<TIn, TOut>>() ?? [])
                .ToList();

            if (evaluationHandlers.Count == 0)
            {
                throw new ArgumentException("No chained handlers were found.");
            }

            ChainedEvaluationHandlerBuilder<TIn, TOut> builder =
                ChainedEvaluationHandlerBuilder<TIn, TOut>.Create(
                    evaluationHandlers[0].ToChainedHandler());

            foreach (IEvaluationHandler<TIn, TOut> handler in evaluationHandlers.Skip(1))
            {
                builder.ChainNext(handler.ToChainedHandler());
            }

            return builder.Build();
        });
        return services;
    }
}

internal static class ChainedEvaluationHandlerExtensions
{
    internal static IChainedEvaluationHandler<TIn, TOut> ToChainedHandler<TIn, TOut>(
        this IEvaluationHandler<TIn, TOut> handler)
        => new ChainedEvaluationHandler<TIn, TOut>(handler);
}
