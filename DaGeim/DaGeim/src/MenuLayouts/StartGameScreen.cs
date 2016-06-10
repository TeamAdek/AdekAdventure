using System;
using System.Collections.Generic;
using System.IO;
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
        private SpriteFont mainFont;
        private SpriteFont font;
        private Selector selector;

        private Button newGameButton = new Button("New Game", new Rectangle(900, 320, 300, 80));
        private Button scoresButton = new Button("Scores", new Rectangle(900, 420, 300, 80));
        private Button creditsButton = new Button("Credits", new Rectangle(900, 520, 300, 80));
        private Button quitButton = new Button("Quit", new Rectangle(900, 620, 300, 80));


        public void Load(ContentManager content)
        {
            background = content.Load<Texture2D>("background2");
            font = content.Load<SpriteFont>("Font");
            mainFont = content.Load<SpriteFont>("MainFont");
            robotImage = content.Load<Texture2D>("Idle");
            newGameButton.Load(content);
            quitButton.Load(content);
            creditsButton.Load(content);
            scoresButton.Load(content);

            selector = new Selector(900, 320);
        }

        public void Draw(SpriteBatch spritebatch)
        {
            // spritebatch.Begin();
            spritebatch.Draw(background, new Rectangle(0, 0, 1280, 720), Color.White);
            spritebatch.Draw(robotImage, new Rectangle(50, 470, 250, 250), Color.White);

            newGameButton.DrawButton(spritebatch, font);
            scoresButton.DrawButton(spritebatch, font);
            creditsButton.DrawButton(spritebatch, font);
            quitButton.DrawButton(spritebatch, font);
            //spritebatch.End();
        }
        //selector = new Selector(900, 620);
        public void Update(GameTime gameTime, MainGame game)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                selector.y -= 20;
                if (selector.y < 320)
                {
                    selector.y = 320;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                selector.y += 20;
                if (selector.y > 720)
                    selector.y = 720;
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
                    //TODO START NEW GAME LOGIC
                }
            }

            if (scoresButton.isSelected)
            {
                //starts Scoreboard
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    //TODO START Scoreboard LOGIC
                }
            }

            if (creditsButton.isSelected)
            {
                //starts Credits
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    //TODO START Credits LOGIC
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
    }
}
