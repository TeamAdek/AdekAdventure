using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RobotBoy.MenuLayouts
{
    public class Button
    {
        private Texture2D highlightedImage;
        private Texture2D darkImage;
        private string buttonText;
        private Rectangle location;
        private bool isSelected;

        public Button(string text, Rectangle location)
        {
            this.buttonText = text;
            this.Location = location;
            this.IsSelected = false;
        }
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                this.isSelected = value;
            }
        }

        public Rectangle Location
        {
            get
            {
                return location;
            }

            private set
            {
                this.location = value;
            }
        }

        public void Load(ContentManager content)
        {
            this.highlightedImage = content.Load<Texture2D>("Menu/HighlightedButton");
            this.darkImage = content.Load<Texture2D>("Menu/DarkButton");
        }

        public void DrawButton(SpriteBatch spriteBatch, SpriteFont font)
        {
            int textX = Location.X + Location.Width/6;
            int textY = Location.Y + (Location.Height*25)/80;
            if (this.IsSelected)
            {
                spriteBatch.Draw(highlightedImage, Location, Color.White);
                spriteBatch.DrawString(font, buttonText, new Vector2(textX, textY),
                    Color.Ivory);
            }
            else
            {
                spriteBatch.Draw(darkImage, Location, Color.White);
                spriteBatch.DrawString(font, buttonText, new Vector2(textX, textY),
                    Color.Ivory);
            }
        }
    }
}
