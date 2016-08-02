using DaGeim.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DaGeim.MenuLayouts
{
    public class MenuScreen : MainGame
    {
        private Texture2D background;
        private Texture2D robotImage;
        private SpriteFont gameNameFont;
        private SpriteFont font;
        private SpriteFont mainFont;
        private SpriteFont creditsFont;

        protected SpriteFont CreditsFont
        {
            get
            {
                return creditsFont;
            }

            private set 
            {
                this.creditsFont = value;
            }
        }

        protected SpriteFont MainFont
        {
            get
            {
                return mainFont;
            }

            private set
            {
                this.mainFont = value;
            }
        }

        protected SpriteFont Font
        {
            get
            {
                return font;
            }

            private set
            {
                this.font = value;
            }
        }

        protected SpriteFont GameNameFont
        {
            get
            {
                return gameNameFont;
            }

            private set
            {
                this.gameNameFont = value;
            }
        }

        public virtual void Load(ContentManager content)
        {
            this.background = content.Load<Texture2D>("background2");
            this.robotImage = content.Load<Texture2D>("Idle");
            this.Font = content.Load<SpriteFont>("Font");
            this.GameNameFont = content.Load<SpriteFont>("GameNameFont");
            this.MainFont = content.Load<SpriteFont>("MainFont");
            this.CreditsFont = content.Load<SpriteFont>("CreditsFont");
        }

        public virtual void Update(GameTime gameTime, MainGame game)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 1280, 720), Color.White);
            spriteBatch.Draw(robotImage, new Rectangle(50, 470, 250, 250), Color.White);
            spriteBatch.End();
        }
    }
}
