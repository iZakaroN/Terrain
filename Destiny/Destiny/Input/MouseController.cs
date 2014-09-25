using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Destiny.Input
{
	public enum MouseEvents : int
	{
		LeftButton,
		MiddleButton,
		RightButton,
		XButton1,
		XButton2,
		ScrollWheel,
		X,
		Y,
		Position
	}

	public class MouseController : BasicController
	{
		static ControllerID MouseID = new ControllerID(1);

		List<PressedEvent> _pressedEvents = new List<PressedEvent>();
		List<MotionEvent> _motionEvents = new List<MotionEvent>();
		Dictionary<MouseEvents, int> _motionValues = new Dictionary<MouseEvents, int>() 
        { 
            { MouseEvents.ScrollWheel, 0 },
            { MouseEvents.X, 0 },
            { MouseEvents.Y, 0 },
        };


		public MouseController(Destiny game)
			: base(game)
		{
			Mouse.WindowHandle = Game.Window.Handle;
		}

		public override ControllerID ID
		{
			get { return MouseID; }
		}

		static public ControllerEvent GetEvent(MouseEvents evnt)
		{
			return new ControllerEvent(MouseID, new EventID((int)evnt));
		}

		public override void Reset()
		{
			var state = Mouse.GetState();
			_motionValues[MouseEvents.ScrollWheel] = state.ScrollWheelValue;
			_motionValues[MouseEvents.X] = state.X;
			_motionValues[MouseEvents.Y] = state.Y;
		}

		public override void Process()
		{
			_pressedEvents.Clear();
			_motionEvents.Clear();


			var state = Mouse.GetState();
			ProcessMotionEvents(state);
			ProcessPressedEvents(state);
		}

		void ProcessPressedEvents(MouseState state)
		{
			if (state.LeftButton == ButtonState.Pressed)
				_pressedEvents.Add(new PressedEvent(GetEvent(MouseEvents.LeftButton)));
			if (state.MiddleButton == ButtonState.Pressed)
				_pressedEvents.Add(new PressedEvent(GetEvent(MouseEvents.MiddleButton)));
			if (state.RightButton == ButtonState.Pressed)
				_pressedEvents.Add(new PressedEvent(GetEvent(MouseEvents.RightButton)));
			if (state.XButton1 == ButtonState.Pressed)
				_pressedEvents.Add(new PressedEvent(GetEvent(MouseEvents.XButton1)));
			if (state.XButton2 == ButtonState.Pressed)
				_pressedEvents.Add(new PressedEvent(GetEvent(MouseEvents.XButton2)));
		}

		void ProcessMotionEvents(MouseState state)
		{
			ProcessMotionEvents(MouseEvents.ScrollWheel, state.ScrollWheelValue);
			ProcessMotionEvents(MouseEvents.X, state.X);
			ProcessMotionEvents(MouseEvents.Y, state.Y);

			CenterMouse(state);

		}

		void ProcessMotionEvents(MouseEvents eventID, int state)
		{
			var oldValue = _motionValues[eventID];
			if (state != oldValue)
			{
				_motionEvents.Add(new MotionEvent(GetEvent(eventID), state - oldValue));
				_motionValues[eventID] = state;
			}
		}

		private void CenterMouse(MouseState ms)
		{
#if !XBOX360
			if (Game.IsActive)
			{
				int cx = Game.Window.ClientBounds.Width / 2;
				int cy = Game.Window.ClientBounds.Height / 2;

				Mouse.SetPosition(cx, cy);

				_motionValues[MouseEvents.X] = cx;
				_motionValues[MouseEvents.Y] = cy;

			}
#endif
		}

		public override List<PressedEvent> PressedEvents
		{
			get { return _pressedEvents; }
		}

		public override List<MotionEvent> MotionEvents
		{
			get { return _motionEvents; }
		}

		public override List<PointingEvent> PointingEvents
		{
			get { return base.PointingEvents; }
		}

	}
}
