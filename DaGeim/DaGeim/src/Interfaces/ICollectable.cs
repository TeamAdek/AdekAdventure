namespace DaGeim.Interfaces
{
    using DaGeim.Entities;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public interface ICollectable
    {
        string ItemType { get; }

        int RestoreHealthPoints { get; }

        int JumpBoost { get; }

        int BonusScorePoints { get; }

        int BonusRockerShootingBooster { get; }

        Vector2 Position { get; set; }
        Rectangle CollisionBox { get; }

        void Load(ContentManager Content);

        void CollisionWithPlayer(Entity entity); // Set from IEntity to Entity

        void Update(GameTime gameTime);

        Rectangle setRectangle(int x, int y, int w, int h);

        void Draw(SpriteBatch spriteBatch);
    }
}