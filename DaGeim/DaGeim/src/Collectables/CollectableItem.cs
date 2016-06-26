using DaGeim.Interfaces;
using Game.src.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DaGeim.src.Collectable
{
    public abstract class CollectableItem : ICollectable
    {
        protected string itemType;
        protected int restoreHealthPoints;
        protected int jumpBoost;
        protected Texture2D spriteTexture;
        protected Vector2 position;
        protected Vector2 velocity;
        protected Rectangle rectangle;
        public bool dead = false;

        public string ItemType
        {
            get { return this.itemType; }
        }

        public int RestoreHealthPoints
        {
            get { return this.restoreHealthPoints; }
        }

        public int JumpBoost
        {
            get { return jumpBoost; }
        }

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Rectangle CollisionBox
        {
            get { return this.rectangle; }
        }

        protected CollectableItem(Vector2 position)
        {
            this.position = position;
            this.rectangle = setRectangle(0, 0, 30, 30);
        }

        public abstract void Load(ContentManager content);

        public void CollisionWithPlayer(IEntity entity)
        {
            if (this.CollisionBox.Intersects(entity.CollisionBox))
            {
                this.Position = new Vector2(-1000, -1000);
                this.restoreHealthPoints = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (velocity.Y < 10)
                velocity.Y += 0.4f;
        }


        public Rectangle setRectangle(int x, int y, int w, int h)
        {
            return new Rectangle((int)Position.X + x, (int)Position.Y + y, w, h);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D t = spriteTexture;
            Vector2 v = Position;
            spriteBatch.Draw(t, v, Color.White);
        }
    }
}