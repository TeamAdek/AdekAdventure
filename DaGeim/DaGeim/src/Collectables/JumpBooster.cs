namespace DaGeim.Collectables
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// JumpBooster is a collectable item, it has a position, itemType and value of jumpBooster power.
    /// It has function Load - loading texture
    /// </summary>
    /// 
    public class JumpBooster : CollectableItem
    {
        public JumpBooster(Vector2 position, string itemType = "jumpBooster") : base(position)
        {
            this.position = position;
            this.itemType = itemType;
            this.jumpBoost = 50;
        }

        public override void Load(ContentManager content)
        {
            this.spriteTexture = content.Load<Texture2D>("pow");
        }
    }
}