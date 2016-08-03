using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RobotBoy.GUI
{
    public class EnemyHealthBar
    {
        private Texture2D hpBar;
        private Vector2 healthPosition;

        private Rectangle healthBar = new Rectangle(0, 0, 1280, 36);

        public void Load(ContentManager content)
        {
            hpBar = content.Load<Texture2D>("GUI/HealthBar");
        }

        public void Update(int hp, Vector2 newPosition)
        {
            double ratio = (320.0f - 90.0f) / (300.0f); // 320 - 90 the two bounds. 
            double result = (hp) * ratio + 90.0f;
            healthBar.Width = (int)result;
            healthPosition.X = newPosition.X - 75; 
            healthPosition.Y = newPosition.Y - 50; 
        }

        public void Draw(SpriteBatch renderEngine)
        {
            renderEngine.Draw(hpBar, healthPosition, healthBar, Color.Wheat);
        }
    }
}
