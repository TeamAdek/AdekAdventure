namespace RobotBoy.Collectables
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class RocketShootingBooster : CollectableItem
    {
        public RocketShootingBooster(Vector2 position) : base(position)
        {
            this.position = position;
            this.bonusRockerShootingBooster = 20;
        }

        public override void Load(ContentManager content)
        {
            spriteTexture = content.Load<Texture2D>("Collectibles/Gun");
        }
    }
}