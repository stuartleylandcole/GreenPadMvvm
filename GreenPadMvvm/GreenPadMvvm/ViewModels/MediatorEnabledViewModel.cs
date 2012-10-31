using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenPadMvvm.Core;

namespace GreenPadMvvm.ViewModels
{
    public abstract class MediatorEnabledViewModel<T> : TabViewModel
    {
        public Mediator<T> Mediator
        {
            get
            {
                return Mediator<T>.Instance;
            }
        }

        public abstract void SubscribeToMessages();
    }
}
