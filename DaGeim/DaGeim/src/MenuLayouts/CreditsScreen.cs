namespace DaGeim.MenuLayouts
{
    using System.Threading;
    using DaGeim.Game;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class CreditsScreen : MenuScreen
    {
        private Button mainMenuButton = new Button("Main Menu", new Rectangle(950, 500, 300, 80));
        private Button quitButton = new Button("Quit", new Rectangle(950, 600, 300, 80));
        /*----------------------------------------------------------------------------------------------
        Loads the content for the menu
        ----------------------------------------------------------------------------------------------*/
        public override void Load(ContentManager content)
        {
            base.Load(content);
            this.mainMenuButton.Load(content);
            this.quitButton.Load(content);
        }
        /*--------------------------------------------------------------------------------------------
        Updates the pause menu when it is active
        ---------------------------------------------------------------------------------------------*/
        public override void Update(GameTime gameTime, MainGame game)
        {
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
            if (this.mainMenuButton.IsSelected)
            {
                if (Mouse.GetState(game.Window).LeftButton == ButtonState.Pressed)
                {
                    GameMenuManager.mainMenuOn = true; //turn on the selected menu
                    GameMenuManager.creditsMenuOn = false; //turn off the current menu
                    GameMenuManager.TurnOtherMenusOff(); // turn off the other menus
                    Thread.Sleep(100);
                }
            }
            if (this.quitButton.IsSelected)
            {
                //the quit button exits the game (you dont say!?!)
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
            spriteBatch.DrawString(base.GameNameFont, "CREDITS", new Vector2(430, 25),
                Color.Ivory);
            spriteBatch.DrawString(base.MainFont, "Team Adek", new Vector2(470, 100),
                Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Player design:", new Vector2(300, 170), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Hristo Hentov & Denis Angelov", new Vector2(400, 200), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Menu design:", new Vector2(300, 230), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Yovko Gospodinov & Iskren Penev", new Vector2(400, 260), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Enemy design:", new Vector2(300, 290), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Simeon Mandazhiev", new Vector2(400, 320), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Level design:", new Vector2(300, 350), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Stefan Todorov", new Vector2(400, 380), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Sound effects:", new Vector2(300, 410), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Ivan Nikolov", new Vector2(400, 440), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Collision:", new Vector2(300, 470), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Alexander Markov", new Vector2(400, 500), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Boss design:", new Vector2(300, 530), Color.Ivory);
            spriteBatch.DrawString(base.CreditsFont, "Denis Angelov", new Vector2(400, 560), Color.Ivory);

            this.mainMenuButton.DrawButton(spriteBatch, base.Font);
            this.quitButton.DrawButton(spriteBatch, base.Font);
            spriteBatch.End();
        }
    }
}

