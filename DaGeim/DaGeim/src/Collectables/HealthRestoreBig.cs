namespace DaGeim.src.Collectable
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// HealthrestoreBig is a collectable item.
    /// HealthrestoreBig inherits Healthrestore. 
    /// It has a position, itemType and more restoreHealthPoints.
    /// It has function Load - loading texture
    /// </summary>
    public class HealthRestoreBig : HealthRestore
    {
        public HealthRestoreBig(Vector2 position, string itemType = "healthRestoreBig")
            : base(position, itemType)
        {
            this.restoreHealthPoints = 100;
        }

        public override void Load(ContentManager content)
        {
            spriteTexture = content.Load<Texture2D>("heart");
        }


    }
}