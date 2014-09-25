using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Input
{
    public struct ControllerID
    {
         int ID;
        public ControllerID(int id)
        {
            ID = id;
        }

        public ControllerID(ControllerID id)
        {
            ID = id.ID;
        }
    }

    public struct EventID
    {
         int ID;
        public EventID(int id)
        {
            ID = id;
        }

        public EventID(EventID id)
        {
            ID = id.ID;
        }
    }

    public struct ControllerEvent
    {
        public ControllerID Controller;
        public EventID Event;

        public ControllerEvent(ControllerID controller, EventID evnt)
        {
            Controller = controller;
            Event = evnt;
        }
    }

    public class BasicEvent
    {
        public ControllerEvent ControllerEvent;
        public BasicEvent(ControllerEvent evnt)
        {
            ControllerEvent = evnt;
        }
    }

    public class PressedEvent : BasicEvent
    {

        public PressedEvent(ControllerEvent evnt)
            : base(evnt)
        {
        }
    }

    public class ClickEvent : BasicEvent
    {

        public ClickEvent(ControllerEvent evnt)
            : base(evnt)
        {
        }
    }

    public class MotionEvent : BasicEvent
    {
        public int Motion;
        public MotionEvent(ControllerEvent evnt, int motion)
            : base(evnt)
        {
            Motion = motion;
        }
    }

    public class PointingEvent : BasicEvent
    {
        public int X;
        public int Y;

        public PointingEvent(ControllerEvent evnt, int x, int y)
            : base(evnt)
        {
            X = x;
            Y = y;
        }
    }
}
