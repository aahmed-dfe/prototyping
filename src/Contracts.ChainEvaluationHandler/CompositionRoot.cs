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
                ChainedEvaluationHandlerBuilder<TIn, TOut>.Create();

            foreach (Func<IServiceProvider, IEvaluationHandler<TIn, TOut>> resolveHandler in resolveableHandlerChain)
            {
                builder.ChainNext(
                    resolveHandler.Invoke(sp).ToChainedHandler());
            }

            return builder.Build();
        });
        return services;
    }

    // Resolve from SP
    // Issue - assumes IEvaluationHandlers have already been registered, no single point of composition
    // Issue - client is registering IEvaluationHandler<TIn,TOut> and resolving, and I'm providing IEvaluationHandler<TIn, TOut> polluted IoC
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
                ChainedEvaluationHandlerBuilder<TIn, TOut>.Create();

            foreach (IEvaluationHandler<TIn, TOut> handler in evaluationHandlers.Skip(1))
            {
                builder.ChainNext(handler.ToChainedHandler());
            }

            return builder.Build();
        });
        return services;
    }

    // Solves - Client is not registering multiple IEvaluationHandlers so no pollution of IoC
    // Solves - Single point of composition
    // Solves - Resolving scoped dependencies from SP enabled where handlers need access to context, not stateless.
    public static IServiceCollection V3AddScopedChainedHandlers<TIn, TOut>(
        IServiceCollection services,
        List<Func<IServiceProvider, IEvaluationHandler<TIn, TOut>>> resolveableHandlerChain)
    {
        services.AddScoped<IEvaluationHandler<TIn, TOut>>(sp =>
        {
            ChainedEvaluationHandlerBuilder<TIn, TOut> builder = ChainedEvaluationHandlerBuilder<TIn, TOut>.Create();

            foreach (Func<IServiceProvider, IEvaluationHandler<TIn, TOut>> handlerExpression in resolveableHandlerChain
                         .Skip(1))
            {
                builder.ChainNext(
                    handlerExpression.Invoke(sp));
            }

            return builder.Build();
        });
        return services;
    }

    // TODO expose the IChainedEvaluationBuilder and not the raw type?
    public static IServiceCollection V4ExposeChainedBuilder<TIn, TOut>(
        IServiceCollection services,
        Action<ChainedEvaluationHandlerBuilder<TIn, TOut>> configureChain) // TODO ServiceProvider extension
    {
        ArgumentNullException.ThrowIfNull(services);
        ChainedEvaluationHandlerBuilder<TIn, TOut> builder = ChainedEvaluationHandlerBuilder<TIn, TOut>.Create();
        configureChain.Invoke(builder);
        services.AddSingleton(builder.Build());
        return services;
    }
}

internal static class ChainedEvaluationHandlerExtensions
{
    internal static IChainedEvaluationHandler<TIn, TOut> ToChainedHandler<TIn, TOut>(
        this IEvaluationHandler<TIn, TOut> handler)
        => new ChainedEvaluationHandler<TIn, TOut>(handler);
}
