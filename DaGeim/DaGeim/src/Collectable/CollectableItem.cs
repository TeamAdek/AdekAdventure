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
        protected Texture2D spriteTexture;
        protected Vector2 position;
        protected Vector2 velocity;
        protected Rectangle rectangle;
        //protected Vector2 startPoint = new Vector2(0, 0);
        public bool dead = false;

        public string ItemType
        {
            get { return this.itemType; }
        }

        public int RestoreHealthPoints
        {
            get { return this.restoreHealthPoints; }
        }

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Rectangle CollisionBox
        {
            get { return this.rectangle; }
            //set { this.rectangle = value; }
        }

        protected CollectableItem(Vector2 position)
        {
            this.position = position;
            //this.startPoint = position;
            //loadAnimations();

        }

        public abstract void Load(ContentManager content);
        
        public void CollisionWithPlayer(IEntity entity)
        {
            if (this.CollisionBox.Intersects(entity.CollisionBox))
            {
                Position = new Vector2(-1000, -1000);
            }
        }

        public void Update(GameTime gameTime)
        {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            rectangle = setRectangle(0, 0, 30, 30);

            if (velocity.Y < 10)
                velocity.Y += 0.4f;

            //base.Update(gameTime);
        }


        public Rectangle setRectangle(int x, int y, int w, int h)
        {
            return new Rectangle((int)Position.X + x, (int)Position.Y + y, w, h);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D t = spriteTexture;
            Vector2 v = Position;
            //Rectangle src = spriteAnimations[currentAnimation][frameIndex];
            spriteBatch.Draw(t, v, Color.White);
            //base.Draw(spriteBach);
        }
    }
}