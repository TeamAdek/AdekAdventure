namespace DaGeim.Collectables
{
    using DaGeim.src.Collectable;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Chest : CollectableItem
    {
        public Chest(Vector2 position) : base(position)
        {
            this.position = position;
            this.bonusScorePoints = 1000;
        }

        public override void Load(ContentManager content)
        {
            spriteTexture = content.Load<Texture2D>("Chest");
        }
    }
}