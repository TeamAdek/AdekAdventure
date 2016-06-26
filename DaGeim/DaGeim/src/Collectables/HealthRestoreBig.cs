using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DaGeim.src.Collectable
{
    public class HealthRestoreBig : CollectableItem
    {
        public HealthRestoreBig(Vector2 position, string itemType = "healthRestoreBig") : base(position)
        {
            this.position = position;
            this.itemType = itemType;
            this.restoreHealthPoints = 100;
        }

        public override void Load(ContentManager content)
        {
            spriteTexture = content.Load<Texture2D>("heart");
        }
    }
}