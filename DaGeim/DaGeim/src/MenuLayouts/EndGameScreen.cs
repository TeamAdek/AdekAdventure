namespace DaGeim.MenuLayouts
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using DaGeim.Game;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class EndGameScreen : MenuScreen
    {
        private List<int> highscores = new List<int>();
        private char[] delimiters = { ' ', '\t', '\n' };
        private string path = @"TextFile1.txt";
        private bool wasUpdated;
        private int positionAtScorelist;
        private Button newGameButton = new Button("New Game", new Rectangle(950, 400, 300, 80));
        private Button mainMenuButton = new Button("Main Menu", new Rectangle(950, 500, 300, 80));
        private Button quitButton = new Button("Quit", new Rectangle(950, 600, 300, 80));
        private Texture2D star;

        /// <summary>
        /// Loads the content for the end game screen
        /// </summary>
        /// <param name="content"></param>
        public override void Load(ContentManager content)
        {
            base.Load(content);
            //load the content for the scoreboard
            this.star = content.Load<Texture2D>("bluestar");
            this.newGameButton.Load(content);
            this.mainMenuButton.Load(content);
            this.quitButton.Load(content);
            //load the current highscores from file and fill a list with them
            using (StreamReader reader = new StreamReader(path))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    int points = int.Parse(tokens[1]);
                    highscores.Add(points);
                }
            }
        }
        /// <summary>
        /// Saves the scores in the file only if the player managed to get in top 10
        /// we are using it in the UpdateScore method below
        /// </summary>
        private void SaveScores()
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (int i = 0; i < 10; i++)
                {
                    string line = string.Format("++   {0}",
                        highscores[i]);
                    writer.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Update the scoreboard each time the player dies or clears the level
        /// </summary>
        /// <param name="playerScore"></param>
        public void UpdateScoreboard(int playerScore)
        {
            wasUpdated = false;
            positionAtScorelist = -1;
            //set to false every time we call the method
            for (int i = 0; i < 10; i++)
            {
                if (playerScore > highscores[i])
                {
                    highscores.Insert(i, playerScore);
                    positionAtScorelist = i;
                    wasUpdated = true;
                    break;
                }
            }
            if (wasUpdated) //remove the last entry in the list and save the changes
            {
                highscores.RemoveAt(10);
            }
            SaveScores();
        }

        /// <summary>
        /// Draws the end game screen if it is active
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            int numberX = 350;
            //int playerNameX = 350;
            int scoreX = 750;
            int rowY = 90;
            spriteBatch.Begin();
            spriteBatch.DrawString(base.MainFont, "HIGHSCORES", new Vector2(500, 25),
                Color.Ivory);
            this.mainMenuButton.DrawButton(spriteBatch, base.Font);
            this.newGameButton.DrawButton(spriteBatch, base.Font);
            this.quitButton.DrawButton(spriteBatch, base.Font);
            //draw the data from the list
            for (int i = 0; i < 10; i++)
            {
                if (wasUpdated && positionAtScorelist == i)
                {
                    spriteBatch.Draw(star, new Rectangle(280, rowY, 40, 40), Color.White);
                }
                string position = string.Format("{0}.", i + 1).PadLeft(3);
                //  string name = highscores[i].playerName;
                string score = highscores[i].ToString().PadLeft(10, ' ');
                spriteBatch.DrawString(base.Font, position, new Vector2(numberX, rowY), Color.Ivory);
                //  spritebatch.DrawString(font, name, new Vector2(playerNameX, rowY),
                // Color.Ivory);
                spriteBatch.DrawString(base.Font, score, new Vector2(scoreX, rowY),
                    Color.Ivory);
                rowY += 55;
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Update the end game screen if it is active
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="game"></param>
        public override void Update(GameTime gameTime, MainGame game)
        {
            if (this.mainMenuButton.Location.Contains(Mouse.GetState(game.Window).Position))
            {
                this.mainMenuButton.IsSelected = true;
            }
            else
            {
               this. mainMenuButton.IsSelected = false;
            }
            if (this.newGameButton.Location.Contains(Mouse.GetState(game.Window).Position))
            {
                this.newGameButton.IsSelected = true;
            }
            else
            {
                this.newGameButton.IsSelected = false;
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
            if (this.mainMenuButton.IsSelected)
            {
                //returns to the main menu screen
                if (Mouse.GetState(game.Window).LeftButton == ButtonState.Pressed)
                {
                    GameMenuManager.mainMenuOn = true; //turn on the selected menu
                    GameMenuManager.endGameMenuOn = false; //turn off the current menu
                    GameMenuManager.TurnOtherMenusOff(); // turn off the other menus
                    Thread.Sleep(100);
                }
            }
            if (this.newGameButton.IsSelected)
            {
                //starts new game
                if (Mouse.GetState(game.Window).LeftButton == ButtonState.Pressed)
                {
                    GameMenuManager.gameOn = true; // turn on the selected menu
                    GameMenuManager.endGameMenuOn = false; //turn off the current menu
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
    }
}
