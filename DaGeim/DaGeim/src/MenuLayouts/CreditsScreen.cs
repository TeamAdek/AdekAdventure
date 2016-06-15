using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DaGeim
{
    public class CreditsScreen : MainGame
    {
        private Button mainMenuButton = new Button("Main Menu", new Rectangle(950, 500, 300, 80));
        private Button quitButton = new Button("Quit", new Rectangle(950, 600, 300, 80));
        private Selector selector;
        private Texture2D background;
        private Texture2D robotImage;
        private SpriteFont gameNameFont;
        private SpriteFont mainFont;
        private SpriteFont creditsFont;
        private SpriteFont font;

        /*----------------------------------------------------------------------------------------------
        Loads the content for the menu
        ----------------------------------------------------------------------------------------------*/
        public void Load(ContentManager content)
        {
            mainMenuButton.Load(content);
            quitButton.Load(content);
            selector = new Selector(1000, 520);
            background = content.Load<Texture2D>("background2");
            robotImage = content.Load<Texture2D>("Idle");
            gameNameFont = content.Load<SpriteFont>("GameNameFont");
            mainFont = content.Load<SpriteFont>("MainFont");
            creditsFont = content.Load<SpriteFont>("CreditsFont");
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
                if (selector.y < 520)
                    selector.y = 520; // the selector cant go above this position
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                selector.y += 25; // we move the selector 20px down the screen
                if (selector.y > 650)
                    selector.y = 650; // the selector cant go below this position
            }
            //make a small rectangle to check the if the selector is inside one of the buttons
            Rectangle rect = new Rectangle(selector.x, selector.y, 25, 25);
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
            if (mainMenuButton.isSelected)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    GameMenuManager.mainMenuOn = true; //turn on the selected menu
                    GameMenuManager.creditsMenuOn = false; //turn off the current menu
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
            spriteBatch.DrawString(gameNameFont, "CREDITS", new Vector2(430, 25),
                Color.Ivory);
            spriteBatch.DrawString(mainFont, "Team Adek", new Vector2(470, 100),
                Color.Ivory);
            spriteBatch.DrawString(creditsFont, "Player design:", new Vector2(300, 170), Color.Ivory);
            spriteBatch.DrawString(creditsFont, "Hristo Hentov & Denis Angelov", new Vector2(400, 200), Color.Ivory);
            spriteBatch.DrawString(creditsFont, "Menu design:", new Vector2(300, 230), Color.Ivory);
            spriteBatch.DrawString(creditsFont, "Yovko Gospodinov & Iskren Penev", new Vector2(400, 260), Color.Ivory);
            spriteBatch.DrawString(creditsFont, "Enemy design:", new Vector2(300, 290), Color.Ivory);
            spriteBatch.DrawString(creditsFont, "Simeon Mandazhiev", new Vector2(400, 320), Color.Ivory);
            spriteBatch.DrawString(creditsFont, "Level design:", new Vector2(300, 350), Color.Ivory);
            spriteBatch.DrawString(creditsFont, "Stefan Todorov", new Vector2(400, 380), Color.Ivory);
            spriteBatch.DrawString(creditsFont, "Sound effects:", new Vector2(300, 410), Color.Ivory);
            spriteBatch.DrawString(creditsFont, "Ivan Nikolov", new Vector2(400, 440), Color.Ivory);
            spriteBatch.DrawString(creditsFont, "Collision:", new Vector2(300, 470), Color.Ivory);
            spriteBatch.DrawString(creditsFont, "Alexander Markov", new Vector2(400, 500), Color.Ivory);

            mainMenuButton.DrawButton(spriteBatch, font);
            quitButton.DrawButton(spriteBatch, font);
            spriteBatch.End();
        }
    }
}

