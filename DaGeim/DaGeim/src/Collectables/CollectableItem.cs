using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RobotBoy.Entities;
using RobotBoy.Interfaces;

namespace RobotBoy.Collectables
{
    /// <summary>
    /// CollectableItem is a base, abstract class for any collectable items.
    /// </summary>
    public abstract class CollectableItem : ICollectable
    {
        protected string itemType;
        protected int restoreHealthPoints;
        protected int jumpBoost;
        protected int bonusScorePoints;
        protected int bonusRockerShootingBooster;
        protected Texture2D spriteTexture;
        protected Vector2 position;
        protected Vector2 velocity;
        protected Rectangle rectangle;
        private bool dead = false;

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
            get { return this.jumpBoost; }
        }
        public int BonusScorePoints
        {
            get { return this.bonusScorePoints; }
        }

        public int BonusRockerShootingBooster
        {
            get { return this.bonusRockerShootingBooster; }
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

        /// <summary>
        /// CollisionWithPlayer method check for collision with player but can work with ani Entity objects.
        /// If collision is detected the CollectableItem hes restoreHealthPoints down to zero and
        /// go out of the screen but already exist.
        /// </summary>
        /// <param name="entity">Method can work with any Entity objects.</param>
        public void CollisionWithPlayer(Entity entity)
        {
            if (this.CollisionBox.Intersects(entity.CollisionBox))
            {
                this.Position = new Vector2(-1000, -1000);
                this.restoreHealthPoints = 0;
                this.bonusScorePoints = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.velocity.Y < 10)
                this.velocity.Y += 0.4f;
        }


        public Rectangle setRectangle(int x, int y, int w, int h)
        {
            return new Rectangle((int)this.Position.X + x, (int)this.Position.Y + y, w, h);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D t = this.spriteTexture;
            Vector2 v = this.Position;
            spriteBatch.Draw(t, v, Color.White);
        }
    }
}