using Microsoft.Extensions.DependencyInjection;

namespace Contracts.Logging;

public static class CompositionRoot
{
    // Logging Sink registers one or more messaged based on a series of predicates over a defined context? Specification?
    // Something must evaluate the predicates when a LogMessage is sent to the sinks, ILogMessageContextFilter?
    // Internal to the Logger, some kind of core LogMessage contract? BusinessEventMessage : ILogMessage, IBusinessEvent
    // An Adaption from LogMessage to LoggingSubscribableContext

    // Mediator that is published to from client. Mediator publishes to? Something is injecting into the Mediator that receives an input to it, What it takes should be the uniform ILogMessage?
    // All handlers are injected (and notified of the created LogMessage), their predicate(s) is evaluated? ILoggingContextEvaluationHandler.

    // LogMessageFormatters? Again may apply globally or on a per-message?

    // 2 types of logs? System and BusinessEvents, System is the only one that can be ratched up/down for Verbosity, BusinessEvents are always published?
    // Some business events may be decorated as auditable (because Auditability is in itself, a "Business" necessary framing)

    // LoggingFactory.ResolveLogger("key")
    // .Log(new SystemLoggingMessage(LogLevel, Message)); (optional override for formatting?)

    // LoggingFactory.Resolve("logger")
    // .Log(new DispatchedEventMessage(properties-essential-to-create));

    // ILogMessage, IBusinessEventMessage, IAuditable.

    public static IServiceCollection AddLoggingCore(this IServiceCollection services)
    {
        //services.AddSingleton<ILoggingMediator>
        return services;
    }

    public static IServiceCollection AddLoggingSink(this IServiceCollection services, Func<ILoggingContext, bool> predicate)
    {
        return services;
    }
}

