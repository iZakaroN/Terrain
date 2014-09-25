using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Destiny.Input.Actions;

namespace Destiny.Input
{
    public interface IActionElement
    {
    }

    public interface IActionElement<TEvent, TAction> : IActionElement
        where TEvent : BasicEvent
        where TAction : ControllerAction<TEvent>
    {
        List<TAction> Actions { get; }
    }

    public interface IMotionActionElement : IActionElement<MotionEvent, MotionAction>
    {
    }

    public interface IPointingActionElement : IActionElement<PointingEvent, PointingAction>
    {
    }

    public interface IPressedActionElement : IActionElement<PressedEvent, PressedAction>
    {
    }

    public interface IClickActionElement : IActionElement<ClickEvent, ClickAction>
    {
    }

}
