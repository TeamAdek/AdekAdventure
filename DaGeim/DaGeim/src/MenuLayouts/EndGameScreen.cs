using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using DaGeim.src.MenuLayouts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DaGeim.MenuLayouts
{
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
        private Selector selector;
        private Texture2D star;

        /*---------------------------------------------------------------------------------------------------
        Loads the content for the end game screen
        ----------------------------------------------------------------------------------------------------*/
        public override void Load(ContentManager content)
        {
            base.Load(content);
            //load the content for the scoreboard
            this.star = content.Load<Texture2D>("bluestar");
            this.newGameButton.Load(content);
            this.mainMenuButton.Load(content);
            this.quitButton.Load(content);
            //give the selector object cords inside the top-most button
            this.selector = new Selector(1000, 420);
            //load the current highscores from file and fill a list with them
            using (StreamReader reader = new StreamReader(path))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    string name = tokens[0];
                    int points = int.Parse(tokens[1]);
                    highscores.Add(points);
                }
            }
        }
        /*-------------------------------------------------------------------------------------------------
        Saves the scores in the file only if the player managed to get in top 10
        we are using it in the UpdateScore method below
        --------------------------------------------------------------------------------------------------*/
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
        /*-------------------------------------------------------------------------------------------------
        Update the scoreboard each time the player dies or clears the level
        -------------------------------------------------------------------------------------------------*/
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
        /*-------------------------------------------------------------------------------------------------
        Draws the end game screen if it is active
        -------------------------------------------------------------------------------------------------*/
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
        /*-------------------------------------------------------------------------------------------------
        Update the end game screen if it is active
        -------------------------------------------------------------------------------------------------*/
        public override void Update(GameTime gameTime, MainGame game)
        {
            //update the position of the selector and change the state of the buttons
            //once a button is active, pressing "space" will do something according the button
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                this.selector.Y -= 25; // we move the selector 20px up the screen
                if (this.selector.Y < 420)
                    this.selector.Y = 420; // the selector cant go above this position
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                this.selector.Y += 25; // we move the selector 20px down the screen
                if (this.selector.Y > 620)
                    this.selector.Y = 620; // the selector cant go below this position
            }

            //make a small rectangle to check the if the selector is inside one of the buttons
            Rectangle rect = new Rectangle(this.selector.X, this.selector.Y, 20, 20);
            if (rect.Intersects(this.mainMenuButton.Location))
            {
                this.mainMenuButton.IsSelected = true;
            }
            else
            {
               this. mainMenuButton.IsSelected = false;
            }
            if (rect.Intersects(this.newGameButton.Location))
            {
                this.newGameButton.IsSelected = true;
            }
            else
            {
                this.newGameButton.IsSelected = false;
            }
            if (rect.Intersects(this.quitButton.Location))
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
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
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
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
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
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    game.Exit();
                }
            }
        }
    }
}
