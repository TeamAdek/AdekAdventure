namespace DaGeim.Entities
{
    using DaGeim.Helper_Classes;
    using DaGeim.Interfaces;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class Entity : AnimatedSprite, IGameObject, ICollidable
    {
        /// <summary>
        /// General Entity Properties.
        /// </summary>
        protected Texture2D entityTexture; // The entity image that is currently rendered on screen.
        protected Vector2 entityPosition;  // The entity position.
        protected Vector2 entityVelocity; // Current entity speed.
        protected Vector2 entityDirection; // Determines how much [-1..0..1] should an entity move on the x and y axis.
        protected Orientations entityOrientation; // Determines which way is the enemy facing currently. up,down,left,right

        public Rectangle CollisionBox { get; protected set; }
        public int Health { get;  set; }
        public bool Dead { get; set; }

        protected enum Orientations{Left,Right,Up,Down}
    
        protected Entity(Vector2 position)
        {
            this.entityPosition = position;
        }

        public Vector2 Position { get { return this.entityPosition; } }

        public abstract void LoadContent(ContentManager content);
        public abstract void Draw(SpriteBatch spriteBatch);
        protected abstract void LoadAnimations();

        protected Rectangle SetCollisionRectangle(int x, int y, int w, int h)
        {
            return new Rectangle((int)this.entityPosition.X + x, (int)this.entityPosition.Y + y, w, h);
        }

        public virtual void CollisionWithMap(Rectangle tileRectangle, int mapWidth, int mapHeight)
        {
            if (this.CollisionBox.TouchTopOf(tileRectangle))
            { 
                this.CollisionBox = new Rectangle(this.CollisionBox.X, tileRectangle.Y - this.CollisionBox.Height, this.CollisionBox.Width, this.CollisionBox.Height);
                this.entityVelocity.Y = 0.0f;
                this.entityDirection.Y = 0.0f;
            }
            if (this.CollisionBox.TouchLeftOf(tileRectangle))
                this.entityPosition.X = tileRectangle.X - tileRectangle.Width - 2;

            if (this.CollisionBox.TouchRightOf(tileRectangle))
                this.entityPosition.X = tileRectangle.X + tileRectangle.Width + 2;

            if (this.CollisionBox.TouchBottomOf(tileRectangle))
                this.entityVelocity.Y = 1.0f;

            if (this.entityPosition.X < 0.0f) this.entityPosition.X = 0.0f;
            if (this.entityPosition.X > mapWidth - this.CollisionBox.Width) this.entityPosition.X = mapWidth - this.CollisionBox.Width;
            if (this.entityPosition.Y < 0.0f) this.entityVelocity.Y = 1.0f;
            if (this.entityPosition.Y > mapHeight - this.CollisionBox.Height) this.entityPosition.Y = mapHeight - this.CollisionBox.Height;
        }

        public virtual void CollisionWithEntity(Entity entity)
        {
            if (this.CollisionBox.Intersects(entity.CollisionBox))
            {
                //TODO: Add Collision Logic
            }
        }
        public virtual void CollisionWithAmmunition(Ammunition.Ammunition ammunition)
        {
            if (this.CollisionBox.Intersects(ammunition.CollisionBox))
            {
                this.Health -= 50;

                if (this.Health <= 0)
                    this.Dead = true;

                ammunition.IsVisible = false;
            }
        }

        public abstract void UpdateCollisionBounds();
    }
}
