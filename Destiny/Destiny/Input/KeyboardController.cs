using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Destiny.Input
{
    class KeyboardController : BasicController
    {
        static ControllerID KeyboardID = new ControllerID(0);
        List<PressedEvent> _pressedEvents = new List<PressedEvent>();
        List<ClickEvent> _clickEvents = new List<ClickEvent>();

        public KeyboardController(Destiny game)
            : base(game)
        {

        }

        public override ControllerID ID
        {
            get { return KeyboardID; }
        }

        static public ControllerEvent GetEvent(Keys key)
        {
            return new ControllerEvent(KeyboardID, new EventID((int)key));
        }

        HashSet<Keys> PressedKeys = new HashSet<Keys>();

        public override void Process()
        {
            _pressedEvents.Clear();
            _clickEvents.Clear();
            var keyState = Keyboard.GetState().GetPressedKeys();
            foreach (var key in keyState)
            {
                _pressedEvents.Add(new PressedEvent(GetEvent(key)));
                if (!PressedKeys.Contains(key))
                    _clickEvents.Add(new ClickEvent(GetEvent(key)));
            }
                    
            PressedKeys = new HashSet<Keys>(keyState);
        }

        public override List<PressedEvent> PressedEvents
        {
            get { return _pressedEvents; }
        }

        public override List<ClickEvent> ClickEvents
        {
            get { return _clickEvents; }
        }

    }
}
