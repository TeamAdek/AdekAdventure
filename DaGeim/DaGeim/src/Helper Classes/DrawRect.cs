using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RobotBoy.Helper_Classes
{
    class DrawRect
    {
        private static Texture2D rectText;

        public static void LoadContent(ContentManager content)
        {
            rectText = content.Load<Texture2D>("Utils/Red");
        
        }
        public static void Draw(Rectangle area, SpriteBatch sb)
        {
            sb.Draw(rectText, area, Color.White);
        }
    }
}
