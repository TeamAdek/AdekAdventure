using System.Threading;
using DaGeim.src.MenuLayouts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DaGeim.MenuLayouts
{
    public class PauseGameScreen : MenuScreen
    {
        private Button resumeGameButton = new Button("Resume Game", new Rectangle(440, 240, 400, 80));
        private Button mainMenuButton = new Button("Main Menu", new Rectangle(440, 350, 400, 80));
        private Button quitButton = new Button("Quit", new Rectangle(440, 460, 400, 80));
        private Selector selector;

        /*----------------------------------------------------------------------------------------------
        Loads the content for the menu
        ----------------------------------------------------------------------------------------------*/
        public override void Load(ContentManager content)
        {
            base.Load(content);
            this.resumeGameButton.Load(content);
            this.mainMenuButton.Load(content);
            this.quitButton.Load(content);
            this.selector = new Selector(450, 250);
           
        }
        /*--------------------------------------------------------------------------------------------
        Updates the pause menu when it is active
        ---------------------------------------------------------------------------------------------*/
        public override void Update(GameTime gameTime, MainGame game)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                this.selector.Y -= 25; // we move the selector 20px up the screen
                if (this.selector.Y < 250)
                    this.selector.Y = 250; // the selector cant go above this position
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                this.selector.Y += 25; // we move the selector 20px down the screen
                if (this.selector.Y > 520)
                    this.selector.Y = 520; // the selector cant go below this position
            }
            //make a small rectangle to check the if the selector is inside one of the buttons
            Rectangle rect = new Rectangle(this.selector.X, this.selector.Y, 25, 25);
            if (rect.Intersects(this.resumeGameButton.Location))
            {
                this.resumeGameButton.IsSelected = true;
            }
            else
            {
                this.resumeGameButton.IsSelected = false;
            }
            if (rect.Intersects(this.mainMenuButton.Location))
            {
                this.mainMenuButton.IsSelected = true;
            }
            else
            {
                this.mainMenuButton.IsSelected = false;
            }
            if (rect.Intersects(this.quitButton.Location))
            {
                this.quitButton.IsSelected = true;
            }
            else
            {
               this. quitButton.IsSelected = false;
            }
            if (this.resumeGameButton.IsSelected)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    GameMenuManager.gameOn = true; //turn on the selected menu
                    GameMenuManager.pauseMenuOn = false;//turn off the current menu
                    GameMenuManager.TurnOtherMenusOff();
                    Thread.Sleep(100);

                }
            }
            if (this.mainMenuButton.IsSelected) // does not work
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    GameMenuManager.mainMenuOn = true; //turn on the selected menu
                    GameMenuManager.pauseMenuOn = false; //turn off the current menu
                    GameMenuManager.TurnOtherMenusOff(); // turn off the other menus
                    Thread.Sleep(100);
                }
            }
            if (this.quitButton.IsSelected)
            {
                //the quit button exits the game (you dont say!?!)
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
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
            this.resumeGameButton.DrawButton(spriteBatch,base.Font);
            this.mainMenuButton.DrawButton(spriteBatch, base.Font);
            this.quitButton.DrawButton(spriteBatch, base.Font);
            spriteBatch.End();
        }
    }
}
