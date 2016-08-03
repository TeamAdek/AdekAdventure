using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RobotBoy.Entities.Ammunition
{
    internal class Laser : Ammunition
    {
        private static Texture2D LeftTexture { get; set; }
        public Laser(Vector2 position, string direction, Texture2D left) :
            base(position, direction)
        {
            Load(left);
            Sprite = LeftTexture;
        }

        public override void LoadContent(ContentManager content)
        {
        }

        public void Load(Texture2D left)
        {
            LeftTexture = left;
        }
    }
}