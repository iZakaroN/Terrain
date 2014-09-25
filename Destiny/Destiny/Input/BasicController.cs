using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Concurrent;
using Destiny.Input.Actions;

namespace Destiny.Input
{
    abstract public class BasicController
    {
        readonly public Destiny Game;

        public ConcurrentDictionary<EventID, MotionAction> MotionActions = new ConcurrentDictionary<EventID, MotionAction>();
        public ConcurrentDictionary<EventID, PointingAction> PointingActions = new ConcurrentDictionary<EventID, PointingAction>();
        public ConcurrentDictionary<EventID, PressedAction> PressedActions = new ConcurrentDictionary<EventID, PressedAction>();
        public ConcurrentDictionary<EventID, ClickAction> ClickActions = new ConcurrentDictionary<EventID, ClickAction>();

        public BasicController(Destiny game)
        {
            Game = game;
        }

        abstract public ControllerID ID { get; }

        virtual public List<MotionEvent> MotionEvents { get { return new List<MotionEvent>(); } }
        virtual public List<PointingEvent> PointingEvents { get { return new List<PointingEvent>(); } }
        virtual public List<PressedEvent> PressedEvents { get { return new List<PressedEvent>(); } }
        virtual public List<ClickEvent> ClickEvents { get { return new List<ClickEvent>(); } }

        public void ProcessEvents(GameTime gameTime)
        {
            Process();
            ProcessEvents(MotionEvents, MotionActions, gameTime);
            ProcessEvents(PointingEvents, PointingActions, gameTime);
            ProcessEvents(PressedEvents, PressedActions, gameTime);
            ProcessEvents(ClickEvents, ClickActions, gameTime);
        }

        public void ProcessEvents<TEvent, TAction>(List<TEvent> events, IDictionary<EventID, TAction> assignedActions, GameTime gameTime)
            where TEvent : BasicEvent
            where TAction : ControllerAction<TEvent>
        {
            events.ForEach(evnt =>
            {
                TAction action;
                if (assignedActions.TryGetValue(evnt.ControllerEvent.Event, out action))
                    action.Action(gameTime, evnt);
            });
        }


        public void Assign(MotionAction action)
        {
            Assign<MotionEvent, MotionAction>(MotionActions, action);
        }

        public void Assign(PointingAction action)
        {
            Assign<PointingEvent, PointingAction>(PointingActions, action);
        }

        public void Assign(PressedAction action)
        {
            Assign<PressedEvent, PressedAction>(PressedActions, action);
        }

        public void Assign(ClickAction action)
        {
            Assign<ClickEvent, ClickAction>(ClickActions, action);
        }

        public void Assign<TEvent, TAction>(ConcurrentDictionary<EventID, TAction> actions, TAction action)
            where TEvent : BasicEvent
            where TAction : ControllerAction<TEvent>
        {
            actions.GetOrAdd(action.MappedEvent.Event, action);
        }

        public void ResetActions()
        {
            Reset();
            MotionActions.Clear();
            PointingActions.Clear();
            PressedActions.Clear();
            ClickActions.Clear();
        }

        public virtual void Process()
        {
        }

        public virtual void Reset()
        {
        }

    }
}
