namespace DaGeim.MenuLayouts
{
    using DaGeim.Interfaces;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Button : IButton
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
            get { return this.isSelected; }
            set { this.isSelected = value; }
        }

        public Rectangle Location
        {
            get { return this.location; }
            private set { this.location = value; }
        }

        public void Load(ContentManager content)
        {
            this.highlightedImage = content.Load<Texture2D>("highlightedButton");
            this.darkImage = content.Load<Texture2D>("darkButton");
        }

        public void DrawButton(SpriteBatch spriteBatch, SpriteFont font)
        {
            int textX = this.Location.X + this.Location.Width / 6;
            int textY = this.Location.Y + (this.Location.Height * 25) / 80;
            if (this.IsSelected)
            {
                spriteBatch.Draw(this.highlightedImage, this.Location, Color.White);
                spriteBatch.DrawString(font, this.buttonText, new Vector2(textX, textY),
                    Color.Ivory);
            }
            else
            {
                spriteBatch.Draw(this.darkImage, this.Location, Color.White);
                spriteBatch.DrawString(font, this.buttonText, new Vector2(textX, textY),
                    Color.Ivory);
            }
        }
    }
}
