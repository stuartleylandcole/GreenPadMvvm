using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenPadMvvm.Core
{
    public sealed class Mediator<T>
    {
        private static readonly Mediator<T> _instance = new Mediator<T>();

        private volatile object locker = new object();

        MultiDictionary<T, Action<Object>> internalList = new MultiDictionary<T, Action<Object>>();

        private Mediator()
        {
        }

        public static Mediator<T> Instance
        {
            get
            {
                return _instance;
            }
        }

        public void Register(T message, Action<Object> callback)
        {
            internalList.AddValue(message, callback);
        }

        public void NotifyColleagues(T message, object args)
        {
            if (internalList.ContainsKey(message))
            {
                foreach (Action<Object> callback in internalList[message])
                {
                    callback(args);
                }
            }
        }
    }
}
