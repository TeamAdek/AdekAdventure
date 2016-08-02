namespace DaGeim.src.Collectable
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Healthrestore is a collectable item, it has a position, itemType and restoreHealthPoints.
    /// It has function Load - loading texture
    /// </summary>
    public class HealthRestore : CollectableItem
    {
        public HealthRestore(Vector2 position, string itemType = "healthRestore") : base(position)
        {
            this.position = position;
            this.itemType = itemType;
            this.restoreHealthPoints = 50;
        }

        public override void Load(ContentManager content)
        {
            spriteTexture = content.Load<Texture2D>("Corazon");
        }
    }
}
