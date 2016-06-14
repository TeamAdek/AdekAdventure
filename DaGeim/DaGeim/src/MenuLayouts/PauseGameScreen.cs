using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DaGeim
{
    public class PauseGameScreen : MainGame
    {
        private Button resumeGameButton = new Button("Resume Game", new Rectangle(440, 240, 400, 80));
        private Button mainMenuButton = new Button("Main Menu", new Rectangle(440, 350, 400, 80));
        private Button quitButton = new Button("Quit", new Rectangle(440, 460, 400, 80));
        private Selector selector;
        private Texture2D background;
        private Texture2D robotImage;
        private SpriteFont mainFont;
        private SpriteFont font;

        /*----------------------------------------------------------------------------------------------
        Loads the content for the menu
        ----------------------------------------------------------------------------------------------*/
        public void Load(ContentManager content)
        {
            resumeGameButton.Load(content);
            mainMenuButton.Load(content);
            quitButton.Load(content);
            selector = new Selector(450, 250);
            background = content.Load<Texture2D>("background2");
            robotImage = content.Load<Texture2D>("Idle");
            mainFont = content.Load<SpriteFont>("MainFont");
            font = content.Load<SpriteFont>("Font");
        }
        /*--------------------------------------------------------------------------------------------
        Updates the pause menu when it is active
        ---------------------------------------------------------------------------------------------*/
        public void Update(GameTime gameTime, MainGame game)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                selector.y -= 25; // we move the selector 20px up the screen
                if (selector.y < 250)
                    selector.y = 250; // the selector cant go above this position
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                selector.y += 25; // we move the selector 20px down the screen
                if (selector.y > 520)
                    selector.y = 520; // the selector cant go below this position
            }
            //make a small rectangle to check the if the selector is inside one of the buttons
            Rectangle rect = new Rectangle(selector.x, selector.y, 25, 25);
            if (rect.Intersects(resumeGameButton.location))
            {
                resumeGameButton.isSelected = true;
            }
            else
            {
                resumeGameButton.isSelected = false;
            }
            if (rect.Intersects(mainMenuButton.location))
            {
                mainMenuButton.isSelected = true;
            }
            else
            {
                mainMenuButton.isSelected = false;
            }
            if (rect.Intersects(quitButton.location))
            {
                quitButton.isSelected = true;
            }
            else
            {
                quitButton.isSelected = false;
            }
            if (resumeGameButton.isSelected)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    GameMenuManager.gameOn = true; //turn on the selected menu
                    GameMenuManager.pauseMenuOn = false;//turn off the current menu
                    GameMenuManager.TurnOtherMenusOff();
                    Thread.Sleep(100);

                }
            }
            if (mainMenuButton.isSelected) // does not work
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    GameMenuManager.mainMenuOn = true; //turn on the selected menu
                    GameMenuManager.pauseMenuOn = false; //turn off the current menu
                    GameMenuManager.TurnOtherMenusOff(); // turn off the other menus
                    Thread.Sleep(100);
                }
            }
            if (quitButton.isSelected)
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
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 1280, 720), Color.White);
            spriteBatch.Draw(robotImage, new Rectangle(50, 450, 250, 250), Color.White);
            spriteBatch.DrawString(mainFont, "Game Paused", new Vector2(440, 100),
                Color.Ivory);
            resumeGameButton.DrawButton(spriteBatch, font);
            mainMenuButton.DrawButton(spriteBatch, font);
            quitButton.DrawButton(spriteBatch, font);
            spriteBatch.End();
        }
    }
}
