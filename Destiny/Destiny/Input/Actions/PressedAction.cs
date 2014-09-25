using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Destiny.Input
{
    public class PressedAction : ControllerAction<PressedEvent>
    {
        public PressedAction(ControllerEvent evnt, Action<GameTime, PressedEvent> action)
            : base(evnt, action)
        {
        }
    }
}
