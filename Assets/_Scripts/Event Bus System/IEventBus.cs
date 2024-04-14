using System;

namespace Ignix.EventBusSystem
{
    public interface IEventBus
    {
        void Register<T>(Action<T> handler) where T : EventArgs;
        void Unregister<T>(Action<T> handler) where T : EventArgs;
        void Send(EventArgs args);
        void ExecuteQueue();
    }
}