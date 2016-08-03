using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RobotBoy.Game;

namespace RobotBoy.MenuLayouts
{
    public class PauseGameScreen : MenuScreen
    {
        private Button resumeGameButton = new Button("Resume Game", new Rectangle(440, 240, 400, 80));
        private Button mainMenuButton = new Button("Main Menu", new Rectangle(440, 350, 400, 80));
        private Button quitButton = new Button("Quit", new Rectangle(440, 460, 400, 80));

        /*----------------------------------------------------------------------------------------------
        Loads the content for the menu
        ----------------------------------------------------------------------------------------------*/
        public override void Load(ContentManager content)
        {
            base.Load(content);
            this.resumeGameButton.Load(content);
            this.mainMenuButton.Load(content);
            this.quitButton.Load(content);
        }
        /*--------------------------------------------------------------------------------------------
        Updates the pause menu when it is active
        ---------------------------------------------------------------------------------------------*/
        public override void Update(GameTime gameTime, MainGame game)
        {
            if (this.resumeGameButton.Location.Contains(Mouse.GetState(game.Window).Position))
            {
                this.resumeGameButton.IsSelected = true;
            }
            else
            {
                this.resumeGameButton.IsSelected = false;
            }
            if (this.mainMenuButton.Location.Contains(Mouse.GetState(game.Window).Position))
            {
                this.mainMenuButton.IsSelected = true;
            }
            else
            {
                this.mainMenuButton.IsSelected = false;
            }
            if (this.quitButton.Location.Contains(Mouse.GetState(game.Window).Position))
            {
                this.quitButton.IsSelected = true;
            }
            else
            {
                this.quitButton.IsSelected = false;
            }
            if (this.resumeGameButton.IsSelected)
            {
                if (Mouse.GetState(game.Window).LeftButton == ButtonState.Pressed)
                {
                    GameMenuManager.gameOn = true; //turn on the selected menu
                    GameMenuManager.pauseMenuOn = false;//turn off the current menu
                    GameMenuManager.TurnOtherMenusOff();
                    Thread.Sleep(100);

                }
            }
            if (this.mainMenuButton.IsSelected)
            {
                if (Mouse.GetState(game.Window).LeftButton == ButtonState.Pressed)
                {
                    GameMenuManager.mainMenuOn = true; //turn on the selected menu
                    GameMenuManager.pauseMenuOn = false; //turn off the current menu
                    GameMenuManager.TurnOtherMenusOff(); // turn off the other menus
                    Thread.Sleep(100);
                }
            }
            if (this.quitButton.IsSelected)
            {
                //the quit button exits the game
                if (Mouse.GetState(game.Window).LeftButton == ButtonState.Pressed)
                {
                    game.Exit();
                }
            }
        }
        /*------------------------------------------------------------------------------------------
        Draws the pause menu screen when it is active
        ------------------------------------------------------------------------------------------*/
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(base.MainFont, "Game Paused", new Vector2(440, 100),
                Color.Ivory);
            this.resumeGameButton.DrawButton(spriteBatch, base.Font);
            this.mainMenuButton.DrawButton(spriteBatch, base.Font);
            this.quitButton.DrawButton(spriteBatch, base.Font);
            spriteBatch.End();
        }
    }
}
