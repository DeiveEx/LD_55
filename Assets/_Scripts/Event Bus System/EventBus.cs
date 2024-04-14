using System;
using System.Collections.Generic;

namespace Ignix.EventBusSystem
{
    public class EventBus : IEventBus
    {
        #region Fields

        private Dictionary<Type, Dictionary<int, Action<EventArgs>>> _handlers = new();
        private Queue<EventArgs> _eventQueue = new();
        private bool _isExecuting;

        #endregion
        
        #region Public Methods

        public void Register<T>(Action<T> handler) where T : EventArgs
        {
            var type = typeof(T);
            
            if(!_handlers.ContainsKey(type))
                _handlers.Add(type, new());
            
            //Since we can't register generic types, we wrap the call into a method
            void Wrapper(EventArgs args)
            {
                handler((T) args);
            }
            
            _handlers[type].Add(handler.GetHashCode(), Wrapper);
        }
        
        public void Unregister<T>(Action<T> handler) where T : EventArgs
        {
            var type = typeof(T);
            
            if(!_handlers.TryGetValue(type, out var handlerCollection))
                return;

            handlerCollection.Remove(handler.GetHashCode());

            if (_handlers[type].Count == 0)
                _handlers.Remove(type);
        }
        
        public void Send(EventArgs args)
        {
            _eventQueue.Enqueue(args);
        }
        
        public void ExecuteQueue()
        {
            if(_isExecuting)
                return;

            _isExecuting = true;

            while (_eventQueue.Count > 0)
            {
                var args = _eventQueue.Dequeue();

                var type = args.GetType();
            
                if(!_handlers.TryGetValue(type, out var handlerCollection))
                    continue;

                foreach (var handler in handlerCollection.Values)
                {
                    handler(args);
                }
            }

            _isExecuting = false;
        }

        #endregion
    }
}
