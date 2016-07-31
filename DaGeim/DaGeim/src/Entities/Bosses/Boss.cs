namespace DaGeim.Entities.Bosses
{
    using System.Collections.Generic;
    using DaGeim.Entities.Ammunition;
    using DaGeim.Helper_Classes;
    using DaGeim.Level;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Boss : AnimatedBoss
    {
        private bool attacking;
        private Vector2 velocity;
        protected Vector2 position;
        private int patrolRange = 150;
        private int bossFPS = 15;
        private string direction = "right";
        private Vector2 startPoint = new Vector2(0, 0);
        private float distanceToPlayer;
        private List<Lasers> lasers = new List<Lasers>();
        private Texture2D shootTextureLeft;
        private bool playDead;
        private int bossHealth = 180;
        private int attackDelay = 20;
        private int laserDelay = 2;
        public float bossDmg = 15;
        private Rectangle rectangle;
        public bool isPushing = false;

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Rectangle CollisionBox
        {
            get { return this.rectangle; }
            set { this.rectangle = value; }
        }

        public List<Lasers> Lasers
        {
            get { return this.lasers; }
            set { this.lasers = value; }
        }


        public Boss(Vector2 position) : base(position)
        {
            this.position = position;
            this.startPoint = position;
            this.loadAnimations();
            this.PlayAnimation("RunRight");
            this.FramesPerSecond = this.bossFPS;
        }

        public void Load(ContentManager Content)
        {
            this.spriteTexture = Content.Load<Texture2D>("SpriteSheetBoss");
            this.shootTextureLeft = Content.Load<Texture2D>("rocketLeft");
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.bossPosition += this.velocity;
            this.rectangle = this.setCollisionBounds();

            if (this.velocity.Y < 10)
                this.velocity.Y += 0.4f;

            if (this.playDead)
                this.PlayAnimation("Dead");

            if (!this.dead)
            {

                if (this.attackDelay > 0)
                    this.attackDelay--;

                this.distanceToPlayer = this.bossPosition.X - playerPosition.X;

                if (this.distanceToPlayer < 500 && this.attackDelay <= 0)
                {
                    this.attacking = true;

                    if (this.distanceToPlayer < 150)
                    {
                        this.PlayAnimation("Push");
                        this.isPushing = true;
                    }
                    else if(!this.playDead)
                    {
                        this.PlayAnimation("Attack");
                        this.Attack();
                    }
                }

                if (this.attackDelay == 0)
                    this.attackDelay = 40;

                if (!this.attacking)
                    this.Patrol();
            }

            this.UpdateAttack();
            base.Update(gameTime);
        }

        public void Attack()
        {
            if (this.laserDelay >= 0)
                this.laserDelay--;

            if (this.laserDelay <= 0)
            {
                Lasers newLaser;

                newLaser = new Lasers(this.shootTextureLeft);
                newLaser.shootPosition = new Vector2(this.bossPosition.X - 15, this.bossPosition.Y + 45);
                newLaser.direction = "left";

                newLaser.isVisible = true;

                // Add laser to list if they are < 5
                if (this.lasers.Count < 5)
                    this.lasers.Add(newLaser);
            }

            if (this.laserDelay == 0)
                this.laserDelay = 2;
        }

        public void UpdateAttack()
        {
            foreach (Lasers laser in this.lasers)
            {
                //Move lasers according to its direction

                laser.shootPosition.X = laser.shootPosition.X - laser.speed;

                //Set lasers to !visible if they have collided or went off screen

                if (laser.shootPosition.X < Camera.centre.X - 640 || laser.shootPosition.X > Camera.centre.X + 640)
                    laser.isVisible = false;
            }

            //Remove lasers if they are not visible

            for (int i = 0; i < this.lasers.Count; i++)
            {
                if (!this.lasers[i].isVisible)
                {
                    this.lasers.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Patrol()
        {
            if ((this.direction == "left") && (this.bossPosition.X > this.startPoint.X - this.patrolRange))
            {
                this.bossPosition.X -= 2;
                this.PlayAnimation("RunLeft");
            }

            if ((this.direction == "left") && (this.bossPosition.X <= this.startPoint.X - this.patrolRange))
            {
                this.PlayAnimation("RunLeft");
                this.direction = "right";
            }

            if ((this.direction == "right") && (this.bossPosition.X < this.startPoint.X + this.patrolRange))
            {
                this.PlayAnimation("RunRight");
                this.bossPosition.X += 2;
            }

            if ((this.direction == "right") && (this.bossPosition.X >= this.startPoint.X + this.patrolRange))
            {
                this.PlayAnimation("RunRight");
                this.direction = "left";
            }
        }

        public void CollisionWithMap(Rectangle tileRectangle, int mapWidth, int mapHeight)
        {
            if (this.rectangle.TouchTopOf(tileRectangle))
            {
                this.rectangle.Y = tileRectangle.Y - this.rectangle.Height;
                this.velocity.Y = 0f;
            }

            if (this.rectangle.TouchLeftOf(tileRectangle))
                this.bossPosition.X = tileRectangle.X - this.rectangle.Width - 2;

            if (this.rectangle.TouchRightOf(tileRectangle))
                this.bossPosition.X = tileRectangle.X + tileRectangle.Width + 2;

            if (this.rectangle.TouchBottomOf(tileRectangle))
                this.velocity.Y = 1f;

            if (this.bossPosition.X < 0) this.bossPosition.X = 0;
            if (this.bossPosition.X > mapWidth - this.rectangle.Width) this.bossPosition.X = mapWidth - this.rectangle.Width;
            if (this.bossPosition.Y < 0) this.velocity.Y = 1f;
            if (this.bossPosition.Y > mapHeight - this.rectangle.Height) this.bossPosition.Y = mapHeight - this.rectangle.Height;
        }

        private Rectangle setCollisionBounds()
        {
            Rectangle output = new Rectangle();
            switch (this.currentAnimation)
            {
                case "RunLeft": output = this.setRectangle(30, 0, 45, 123); break;
                case "RunRight": output = this.setRectangle(30, 0, 45, 123); break;
                case "Attack": output = this.setRectangle(30, 0, 45, 123); break;
                case "Push": output = this.setRectangle(30, 0, 45, 175); break;
                default: output = this.setRectangle(30, 0, 45, 123); break;
            }
            return output;
        }

        private Rectangle setRectangle(int x, int y, int w, int h)
        {
            return new Rectangle((int)this.bossPosition.X + x, (int)this.bossPosition.Y + y, w, h);
        }

        public override void Draw(SpriteBatch spriteBach)
        {
            foreach (Lasers laser in this.lasers)
                laser.Draw(spriteBach);
            base.Draw(spriteBach);
        }

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack"))
            {
                this.attacking = false;
                this.isPushing = false;
            }

            if (animation.Contains("Dead"))
                this.dead = true;
        }

        private void loadAnimations()
        {
            this.AddAnimation("RunLeft", 12, 0, 0);
            this.AddAnimation("RunRight", 12, 1, 0);
            this.AddAnimation("Attack", 12, 2, 0);
            this.AddAnimation("Dead", 12, 3, 0);
            this.AddAnimation("Push", 12, 4, 47);
        }

        public void CollisionWithRocket(Rocket rocket)
        {
            if (this.CollisionBox.Intersects(rocket.CollisionBox))
            {
                if (this.bossHealth <= 0)
                    this.playDead = true;
                else
                    this.bossHealth -= 20;
                rocket.IsVisible = false;
            }
        }
    }
}
