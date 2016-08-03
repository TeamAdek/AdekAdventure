using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RobotBoy.Entities.Player;

namespace RobotBoy.GUI
{
    internal class HUD
    {
        private Texture2D HUDTexture;
        private Texture2D HPBar;
        private SpriteFont font;
        private Vector2 scorePosition;
        private Vector2 healthPosition;

        private Rectangle healthBar = new Rectangle(0, 0, 1280, 720);
        private Rectangle health = new Rectangle(0, 0, 1280, 720);
        public void Load(ContentManager content)
        {
            HUDTexture = content.Load<Texture2D>("GUI/Hud");
            HPBar = content.Load<Texture2D>("GUI/HealthBar");
            font = content.Load<SpriteFont>("Fonts/Font");
        }

        public void Update(int hp , Vector2 newPosition)
        {
            /// lower bound - 90 , higher bound 320
            double ratio = (320.0f - 90.0f) / (300.0f); // 320 - 90 the two bounds. 300 - player max hp
            double result = (hp)*ratio + 90.0f;
            healthBar.Width = (int)result;
            healthPosition.X = newPosition.X - (1280 / 2); // game width / 2
            healthPosition.Y = newPosition.Y - (720 / 2); // game height / 2
            scorePosition.X = newPosition.X - 535;
            scorePosition.Y = newPosition.Y - 313;

        }
        public void Draw(SpriteBatch renderEngine, Player player)
        {
            Color scoreColor = new Color(new Vector3(
                0.4f,0.9f,0.0f));
            renderEngine.Draw(HPBar, healthPosition, healthBar, Color.White); // Dynamic HP bar
            renderEngine.Draw(HUDTexture, healthPosition, health, Color.AliceBlue); // UI
            renderEngine.DrawString(font, player.Score.ToString(), scorePosition, scoreColor,
                0, new Vector2(0,0),
                new Vector2(0.7f,0.7f),SpriteEffects.None, 1);
        }
    }
}
