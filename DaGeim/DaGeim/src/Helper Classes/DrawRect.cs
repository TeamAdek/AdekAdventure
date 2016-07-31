namespace DaGeim.Helper_Classes
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class DrawRect
    {
        private static Texture2D rectText;

        public static void LoadContent(ContentManager content)
        {
            rectText = content.Load<Texture2D>("red");
        
        }
        public static void Draw(Rectangle area, SpriteBatch sb)
        {
            sb.Draw(rectText, area, Color.White);
        }
    }
}
