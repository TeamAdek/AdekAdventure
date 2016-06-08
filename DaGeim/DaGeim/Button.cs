using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DaGeim
{
    public class Button : Game1
    {
        private Texture2D highlightedImage;
        private Texture2D darkImage;
        private string buttonText;
        public Rectangle location;
        public bool isSelected;

        public Button(string text, Rectangle location)
        {
            buttonText = text;
            this.location = location;
        }

        public Button(string text, Rectangle location, bool selected)
        {
            buttonText = text;
            this.location = location;
            isSelected = selected;
        }

        public void Load(ContentManager content)
        {
            highlightedImage = content.Load<Texture2D>("highlightedButton");
            darkImage = content.Load<Texture2D>("darkButton");
        }

        public void DrawButton(SpriteBatch spriteBatch, SpriteFont font)
        {
            int textX = location.X + location.Width/6;
            int textY = location.Y + (location.Height*25)/80;
            if (isSelected)
            {
                spriteBatch.Draw(highlightedImage, location, Color.White);
                spriteBatch.DrawString(font, buttonText, new Vector2(textX, textY),
                    Color.Ivory);
            }
            else
            {
                spriteBatch.Draw(darkImage, location, Color.White);
                spriteBatch.DrawString(font, buttonText, new Vector2(textX, textY),
                    Color.Ivory);
            }
        }
    }
}
