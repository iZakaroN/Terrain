using Destiny.Graphics.UI;
using Destiny.Input.Actions;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destiny.Input
{
	public class InputControlMapper : IClickActionElement, IPressedActionElement, IMotionActionElement
	{
		Destiny _game;
		Avatar _avatar;
		MainUI _mainUI;
		public InputControlMapper(Destiny game, Avatar avatar, MainUI mainUI)
		{
			_game = game;
			_avatar = avatar;
			_mainUI = mainUI;
		}

		List<ClickAction> IActionElement<ClickEvent, ClickAction>.Actions
		{
			get
			{
				return new List<ClickAction>()
					{
						//Destiny
						new ClickAction(KeyboardController.GetEvent(Keys.F5), (gt,e) => _game.SwitchSolid() ), 
						new ClickAction(KeyboardController.GetEvent(Keys.F6), (gt,e) => _game.SwitchCull() ), 
						//MainUI
						new ClickAction(KeyboardController.GetEvent(Keys.Escape), (gt,e) => _mainUI.Exit()), 
						new ClickAction(KeyboardController.GetEvent(Keys.F1), (gt,e) => _mainUI.Switch()), 
						new ClickAction(KeyboardController.GetEvent(Keys.F3), (gt,e) => _mainUI.BouncingUI.Switch()), 
						new ClickAction(KeyboardController.GetEvent(Keys.F2), (gt,e) => _mainUI.DebugUI.Switch()), 
						new ClickAction(KeyboardController.GetEvent(Keys.F4), (gt,e) => _game.World.SwitchTerrain()), 
						new ClickAction(KeyboardController.GetEvent(Keys.Q), (gt,e) => _game.World.Terrain.SwitchPointing()), 


					};
			}
		}

		List<PressedAction> IActionElement<PressedEvent, PressedAction>.Actions
		{
			get
			{
				return new List<PressedAction>()
                {
					//Avatar
                    new PressedAction(KeyboardController.GetEvent(Keys.Left), _avatar.RotateLeft), 
                    new PressedAction(KeyboardController.GetEvent(Keys.Right), _avatar.RotateRight), 
                    new PressedAction(KeyboardController.GetEvent(Keys.Up), _avatar.RotateUp), 
                    new PressedAction(KeyboardController.GetEvent(Keys.Down), _avatar.RotateDown), 

                    new PressedAction(KeyboardController.GetEvent(Keys.W), _avatar.MoveForward), 
                    new PressedAction(KeyboardController.GetEvent(Keys.S), _avatar.MoveBackward), 
                    new PressedAction(KeyboardController.GetEvent(Keys.A), _avatar.MoveLeft), 
                    new PressedAction(KeyboardController.GetEvent(Keys.D), _avatar.MoveRight), 

                    new PressedAction(KeyboardController.GetEvent(Keys.Space), _avatar.MoveUp), 
                    new PressedAction(KeyboardController.GetEvent(Keys.LeftControl), _avatar.MoveDown), 
                    new PressedAction(MouseController.GetEvent(MouseEvents.RightButton), (gt,pe) => _avatar.SetMapPoint()), 
                };
			}
		}

		List<MotionAction> IActionElement<MotionEvent, MotionAction>.Actions
		{
			get
			{
				return new List<MotionAction>()
                {
					//Avatar
                    new MotionAction(MouseController.GetEvent(MouseEvents.X), _avatar.RotateV), 
                    new MotionAction(MouseController.GetEvent(MouseEvents.Y), _avatar.RotateH), 
                };

			}
		}


	}
}
