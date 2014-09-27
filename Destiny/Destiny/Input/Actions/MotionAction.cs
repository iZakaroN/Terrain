using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Destiny.Input.Actions
{
    public class MotionAction : ControllerAction<MotionEvent>
    {
        public MotionAction(ControllerEvent evnt, Action<GameTime, MotionEvent> action)
            : base(evnt, action)
        {
        }
    }
}
