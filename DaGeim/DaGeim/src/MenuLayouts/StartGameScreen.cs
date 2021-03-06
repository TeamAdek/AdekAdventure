﻿using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RobotBoy.Game;

namespace RobotBoy.MenuLayouts
{
    public class StartGameScreen : MenuScreen
    {
        private Button newGameButton = new Button("New Game", new Rectangle(465, 240, 350, 80));
        private Button scoresButton = new Button("Scores", new Rectangle(465, 360, 350, 80));
        private Button creditsButton = new Button("Credits", new Rectangle(465, 480, 350, 80));
        private Button quitButton = new Button("Quit", new Rectangle(465, 600, 350, 80));

        /*---------------------------------------------------------------------------------------------------
        Loads the content for the start game screen
        ----------------------------------------------------------------------------------------------------*/
        public override void Load(ContentManager content)
        {
            base.Load(content);
            this.newGameButton.Load(content);
            this.quitButton.Load(content);
            this.creditsButton.Load(content);
            this.scoresButton.Load(content);
        }

        /*-------------------------------------------------------------------------------------------------
        Update the start game screen if it is active
        -------------------------------------------------------------------------------------------------*/
        public override void Update(GameTime gameTime, MainGame game)
        {
            if (this.newGameButton.Location.Contains(Mouse.GetState(game.Window).Position))
            {
                this.newGameButton.IsSelected = true;
            }
            else
            {
                this.newGameButton.IsSelected = false;
            }

            if (this.creditsButton.Location.Contains(Mouse.GetState(game.Window).Position))
            {
                this.creditsButton.IsSelected = true;
            }
            else
            {
                this.creditsButton.IsSelected = false;
            }

            if (this.scoresButton.Location.Contains(Mouse.GetState(game.Window).Position))
            {
                this.scoresButton.IsSelected = true;
            }
            else
            {
                this.scoresButton.IsSelected = false;
            }

            if (this.quitButton.Location.Contains(Mouse.GetState(game.Window).Position))
            {
                this.quitButton.IsSelected = true;
            }
            else
            {
                this.quitButton.IsSelected = false;
            }

            //check which button is selected and do the action
            if (this.newGameButton.IsSelected)
            {
                //starts new game
                if (Mouse.GetState(game.Window).LeftButton == ButtonState.Pressed)
                {
                    GameMenuManager.gameOn = true;
                    GameMenuManager.mainMenuOn = false;
                    GameMenuManager.TurnOtherMenusOff();
                    Thread.Sleep(100);
                }
            }
            else if (this.scoresButton.IsSelected)
            {
                //starts Scoreboard
                if (Mouse.GetState(game.Window).LeftButton == ButtonState.Pressed)
                {
                    GameMenuManager.endGameMenuOn = true;
                    GameMenuManager.mainMenuOn = false; //turn the current menu off
                    GameMenuManager.TurnOtherMenusOff(); // turn the other menus off
                    Thread.Sleep(100);
                }
            }
            else if (this.creditsButton.IsSelected)
            {
                //starts Credits
                if (Mouse.GetState(game.Window).LeftButton == ButtonState.Pressed)
                {
                    GameMenuManager.creditsMenuOn = true;
                    GameMenuManager.mainMenuOn = false;//turn the current menu off
                    GameMenuManager.TurnOtherMenusOff(); // turn the other menus off
                    Thread.Sleep(100);
                }

            }

            else if (this.quitButton.IsSelected)
            {
                //the quit button exits the game (you dont say!?!)
                if (Mouse.GetState(game.Window).LeftButton == ButtonState.Pressed)
                {
                    game.Exit();
                }
            }
        }
        /*-------------------------------------------------------------------------------------------------
        Draws the start game screen if it is active
        -------------------------------------------------------------------------------------------------*/
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(base.GameNameFont, "ROBOT BOY", new Vector2(350, 25),
                Color.Ivory);
            this.newGameButton.DrawButton(spriteBatch, base.Font);
            this.scoresButton.DrawButton(spriteBatch, base.Font);
            this.creditsButton.DrawButton(spriteBatch, base.Font);
            this.quitButton.DrawButton(spriteBatch, base.Font);
            spriteBatch.End();
        }

    }
}
