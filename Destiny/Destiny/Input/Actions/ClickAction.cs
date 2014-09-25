using Destiny.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Input.Actions
{
    public class ClickAction : ControllerAction<ClickEvent>
    {
        public ClickAction(ControllerEvent evnt, Action<GameTime, ClickEvent> action)
            : base(evnt, action)
        {
        }
    }
}
