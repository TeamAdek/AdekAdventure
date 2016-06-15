using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DaGeim
{
    public class StartGameScreen : MainGame
    {
        private Texture2D background;
        private Texture2D robotImage;
        private SpriteFont gameNameFont;
        private SpriteFont font;
        private Selector selector;
        private Button newGameButton = new Button("New Game", new Rectangle(465, 240, 350, 80));
        private Button scoresButton = new Button("Scores", new Rectangle(465, 360, 350, 80));
        private Button creditsButton = new Button("Credits", new Rectangle(465, 480, 350, 80));
        private Button quitButton = new Button("Quit", new Rectangle(465, 600, 350, 80));

        /*---------------------------------------------------------------------------------------------------
        Loads the content for the start game screen
        ----------------------------------------------------------------------------------------------------*/
        public void Load(ContentManager content)
        {
            background = content.Load<Texture2D>("background2");
            font = content.Load<SpriteFont>("Font");
            gameNameFont = content.Load<SpriteFont>("GameNameFont");
            robotImage = content.Load<Texture2D>("Idle");
            newGameButton.Load(content);
            quitButton.Load(content);
            creditsButton.Load(content);
            scoresButton.Load(content);

            selector = new Selector(490, 250);
        }
        /*-------------------------------------------------------------------------------------------------
        Draws the start game screen if it is active
        -------------------------------------------------------------------------------------------------*/
        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Begin();
            spritebatch.Draw(background, new Rectangle(0, 0, 1280, 720), Color.White);
            spritebatch.Draw(robotImage, new Rectangle(50, 470, 250, 250), Color.White);
            spritebatch.DrawString(gameNameFont, "ROBOT BOY", new Vector2(350, 25),
                Color.Ivory);

            newGameButton.DrawButton(spritebatch, font);
            scoresButton.DrawButton(spritebatch, font);
            creditsButton.DrawButton(spritebatch, font);
            quitButton.DrawButton(spritebatch, font);
            spritebatch.End();
        }
        /*-------------------------------------------------------------------------------------------------
        Update the start game screen if it is active
        -------------------------------------------------------------------------------------------------*/
        public void Update(GameTime gameTime, MainGame game)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                selector.y -= 25;
                if (selector.y < 250)
                {
                    selector.y = 250;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                selector.y += 25;
                if (selector.y > 650)
                    selector.y = 650;
            }

            //make a small rectangle to check the if the selector is inside one of the buttons

            Rectangle rect = new Rectangle(selector.x, selector.y, 20, 20);
            if (rect.Intersects(newGameButton.location))
            {
                newGameButton.isSelected = true;
            }
            else
            {
                newGameButton.isSelected = false;
            }

            if (rect.Intersects(creditsButton.location))
            {
                creditsButton.isSelected = true;
            }
            else
            {
                creditsButton.isSelected = false;
            }

            if (rect.Intersects(scoresButton.location))
            {
                scoresButton.isSelected = true;
            }
            else
            {
                scoresButton.isSelected = false;
            }

            if (rect.Intersects(quitButton.location))
            {
                quitButton.isSelected = true;
            }
            else
            {
                quitButton.isSelected = false;
            }

            //check which button is selected and do the action
            if (newGameButton.isSelected)
            {
                //starts new game
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    GameMenuManager.gameOn = true;
                    GameMenuManager.mainMenuOn = false;
                    GameMenuManager.TurnOtherMenusOff();
                    Thread.Sleep(100);
                }
            }
            else if (scoresButton.isSelected)
            {
                //starts Scoreboard
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    GameMenuManager.endGameMenuOn = true;
                    GameMenuManager.mainMenuOn = false; //turn the current menu off
                    GameMenuManager.TurnOtherMenusOff(); // turn the other menus off
                    Thread.Sleep(100);
                }
            }
            else if (creditsButton.isSelected)
            {
                //starts Credits
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        GameMenuManager.creditsMenuOn = true;
                        GameMenuManager.mainMenuOn = false;//turn the current menu off
                        GameMenuManager.TurnOtherMenusOff(); // turn the other menus off
                        Thread.Sleep(100);
                    }
                }
            }

            else if (quitButton.isSelected)
            {
                //the quit button exits the game (you dont say!?!)
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    game.Exit();
                }
            }
        }
    }
}
