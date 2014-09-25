using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Destiny.Input
{
    public class PointingAction : ControllerAction<PointingEvent>
    {
        public PointingAction(ControllerEvent evnt, Action<GameTime, PointingEvent> action)
            : base(evnt, action)
        {
        }
    }
}
