namespace Shironeko.Core.Routing;

public interface IEventHandler<in T>
{
    Task Handle(T @event, IEventContext context);
}