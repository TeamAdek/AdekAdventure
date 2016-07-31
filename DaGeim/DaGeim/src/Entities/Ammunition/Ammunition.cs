namespace DaGeim.Entities.Ammunition
{
    using DaGeim.Level;
    using Interfaces;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class Ammunition : IGameObject, ICollidable
    {
        public Vector2 Position { get; set; }

        protected Texture2D Sprite { get; set; }
        public bool IsVisible { get; set; }
        protected float Velocity { get; set; }
        protected string Direction { get; set; }
        public Rectangle CollisionBox { get; protected set; }
        protected Ammunition(Vector2 position, string direction)
        {
            Velocity = 10.0f;
            IsVisible = true;
            Direction = direction;
            Position = position;
        }
        public abstract void LoadContent(ContentManager content);

        public virtual void Update(GameTime gameTime)
        {
            if (Direction == "left")
                Position = new Vector2(Position.X - Velocity, Position.Y);
            else
                Position = new Vector2(Position.X + Velocity, Position.Y);

            if (Position.X < Camera.centre.X - 640 || Position.X > Camera.centre.X + 640)
                IsVisible = false;

            UpdateCollisionBounds();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, Position, Color.White);
        }

        protected Rectangle SetCollisionRectangle(int x, int y, int w, int h)
        {
            return new Rectangle((int)Position.X + x, (int)Position.Y + y, w, h);
        }

        public void UpdateCollisionBounds()
        {
            CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
        }

        public void CollisionWithMap(Rectangle tileCollisionBox, int mapWidth, int mapHeight)
        {
            if (CollisionBox.Intersects(tileCollisionBox))
                IsVisible = false;
        }

        public void CollisionWithEntity(Entity entity) { }
        public void CollisionWithAmmunition(Ammunition ammunition)
        {
            throw new System.NotImplementedException();
        }
    }

}
