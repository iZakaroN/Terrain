using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Destiny.Extensions;
using System.Collections.Concurrent;

namespace Destiny.Input
{
    public class Controller
    {

        readonly public Destiny Game;

        List<IMotionActionElement> _motionActions = new List<IMotionActionElement>();
        List<IPointingActionElement> _pointingActions = new List<IPointingActionElement>();
        List<IPressedActionElement> _pressedActions = new List<IPressedActionElement>();
        List<IClickActionElement> _clickActions = new List<IClickActionElement>();

        Dictionary<ControllerID, BasicController> _controllers = new Dictionary<ControllerID, BasicController>();
        public Controller(Destiny game)
	    {
            Game = game;
            Add(new KeyboardController(Game));
            Add(new MouseController(Game));
	    }

        public void Add(BasicController controller)
        {
            _controllers.Add(controller.ID, controller);
        }

        //Insert actions at the beggining
        public void Register(IActionElement actionElement)
        {
            IMotionActionElement motion = actionElement as IMotionActionElement;
            if (motion!=null)
                _motionActions.Insert(0, motion);

            IPointingActionElement pointing = actionElement as IPointingActionElement;
            if (pointing!=null)
                _pointingActions.Insert(0, pointing);

            IPressedActionElement pressed = actionElement as IPressedActionElement;
            if (pressed!=null)
                _pressedActions.Insert(0, pressed);

            IClickActionElement click = actionElement as IClickActionElement;
            if (click != null)
                _clickActions.Insert(0, click);

            ResetAssignedActions();
        }

        public void Update(GameTime gameTime)
        {
            AssignActions();
            foreach(var controller in _controllers.Values)
                    controller.ProcessEvents(gameTime);
        }

         void AssignActions()
        {
            _motionActions.ForEach(element => AssignActions(element.Actions, (controller, action) => controller.Assign(action)));
            _pointingActions.ForEach(element => AssignActions(element.Actions, (controller, action) => controller.Assign(action)));
            _pressedActions.ForEach(element => AssignActions(element.Actions, (controller, action) => controller.Assign(action)));
            _clickActions.ForEach(element => AssignActions(element.Actions, (controller, action) => controller.Assign(action)));
            
        }

         void AssignActions<TAction>(List<TAction> actions, Action<BasicController,TAction> assign)
            where TAction : ControllerActionBase
        {
            actions.ForEach(action =>
            {
                if (!_controllers.ContainsKey(action.MappedEvent.Controller))
                    ExceptionHelper.Throw("Cannot attach controller {0} to action {1}.", action.MappedEvent.Controller, action);
                assign(_controllers[action.MappedEvent.Controller], action);
            });
        }

         void ResetAssignedActions()
        {
            foreach(var controller in _controllers)
                  controller.Value.ResetActions();
        }


    }
}
