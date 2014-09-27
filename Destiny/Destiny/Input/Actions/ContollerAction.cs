using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Destiny.Input.Actions
{

    abstract public class ControllerActionBase
    {
        public ControllerEvent MappedEvent;

        public ControllerActionBase(ControllerEvent mappedEvent)
        {
            MappedEvent = mappedEvent;
        }

    }

    abstract public class ControllerAction<TEvent> : ControllerActionBase
        where TEvent : BasicEvent
    {
        public Action<GameTime, TEvent> Action;

        public ControllerAction(ControllerEvent mappedEvent, Action<GameTime, TEvent> action)
            : base(mappedEvent)
        {
            Action = action;
        }

    }
}
