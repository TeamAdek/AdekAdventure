using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaGeim.src.Entities.New_Code;

namespace DaGeim
{
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
            startPoint = position;
            loadAnimations();
            PlayAnimation("RunRight");
            FramesPerSecond = bossFPS;
        }

        public void Load(ContentManager Content)
        {
            spriteTexture = Content.Load<Texture2D>("SpriteSheetBoss");
            shootTextureLeft = Content.Load<Texture2D>("rocketLeft");
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            bossPosition += velocity;
            rectangle = setCollisionBounds();

            if (velocity.Y < 10)
                velocity.Y += 0.4f;

            if (playDead)
                PlayAnimation("Dead");

            if (!dead)
            {

                if (attackDelay > 0)
                    attackDelay--;

                distanceToPlayer = bossPosition.X - playerPosition.X;

                if (distanceToPlayer < 500 && attackDelay <= 0)
                {
                    attacking = true;

                    if (distanceToPlayer < 150)
                    {
                        PlayAnimation("Push");
                        isPushing = true;
                    }
                    else if(!playDead)
                    {
                        PlayAnimation("Attack");
                        Attack();
                    }
                }

                if (attackDelay == 0)
                    attackDelay = 40;

                if (!attacking)
                    Patrol();
            }

            UpdateAttack();
            base.Update(gameTime);
        }

        public void Attack()
        {
            if (laserDelay >= 0)
                laserDelay--;

            if (laserDelay <= 0)
            {
                Lasers newLaser;

                newLaser = new Lasers(shootTextureLeft);
                newLaser.shootPosition = new Vector2(bossPosition.X - 15, bossPosition.Y + 45);
                newLaser.direction = "left";

                newLaser.isVisible = true;

                // Add laser to list if they are < 5
                if (lasers.Count < 5)
                    lasers.Add(newLaser);
            }

            if (laserDelay == 0)
                laserDelay = 2;
        }

        public void UpdateAttack()
        {
            foreach (Lasers laser in lasers)
            {
                //Move lasers according to its direction

                laser.shootPosition.X = laser.shootPosition.X - laser.speed;

                //Set lasers to !visible if they have collided or went off screen

                if (laser.shootPosition.X < Camera.centre.X - 640 || laser.shootPosition.X > Camera.centre.X + 640)
                    laser.isVisible = false;
            }

            //Remove lasers if they are not visible

            for (int i = 0; i < lasers.Count; i++)
            {
                if (!lasers[i].isVisible)
                {
                    lasers.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Patrol()
        {
            if ((direction == "left") && (bossPosition.X > startPoint.X - patrolRange))
            {
                bossPosition.X -= 2;
                PlayAnimation("RunLeft");
            }

            if ((direction == "left") && (bossPosition.X <= startPoint.X - patrolRange))
            {
                PlayAnimation("RunLeft");
                direction = "right";
            }

            if ((direction == "right") && (bossPosition.X < startPoint.X + patrolRange))
            {
                PlayAnimation("RunRight");
                bossPosition.X += 2;
            }

            if ((direction == "right") && (bossPosition.X >= startPoint.X + patrolRange))
            {
                PlayAnimation("RunRight");
                direction = "left";
            }
        }

        public void CollisionWithMap(Rectangle tileRectangle, int mapWidth, int mapHeight)
        {
            if (rectangle.TouchTopOf(tileRectangle))
            {
                rectangle.Y = tileRectangle.Y - rectangle.Height;
                velocity.Y = 0f;
            }

            if (rectangle.TouchLeftOf(tileRectangle))
                bossPosition.X = tileRectangle.X - rectangle.Width - 2;

            if (rectangle.TouchRightOf(tileRectangle))
                bossPosition.X = tileRectangle.X + tileRectangle.Width + 2;

            if (rectangle.TouchBottomOf(tileRectangle))
                velocity.Y = 1f;

            if (bossPosition.X < 0) bossPosition.X = 0;
            if (bossPosition.X > mapWidth - rectangle.Width) bossPosition.X = mapWidth - rectangle.Width;
            if (bossPosition.Y < 0) velocity.Y = 1f;
            if (bossPosition.Y > mapHeight - rectangle.Height) bossPosition.Y = mapHeight - rectangle.Height;
        }

        private Rectangle setCollisionBounds()
        {
            Rectangle output = new Rectangle();
            switch (currentAnimation)
            {
                case "RunLeft": output = setRectangle(30, 0, 45, 123); break;
                case "RunRight": output = setRectangle(30, 0, 45, 123); break;
                case "Attack": output = setRectangle(30, 0, 45, 123); break;
                case "Push": output = setRectangle(30, 0, 45, 175); break;
                default: output = setRectangle(30, 0, 45, 123); break;
            }
            return output;
        }

        private Rectangle setRectangle(int x, int y, int w, int h)
        {
            return new Rectangle((int)bossPosition.X + x, (int)bossPosition.Y + y, w, h);
        }

        public override void Draw(SpriteBatch spriteBach)
        {
            foreach (Lasers laser in lasers)
                laser.Draw(spriteBach);
            base.Draw(spriteBach);
        }

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack"))
            {
                attacking = false;
                isPushing = false;
            }

            if (animation.Contains("Dead"))
                dead = true;
        }

        private void loadAnimations()
        {
            AddAnimation("RunLeft", 12, 0, 0);
            AddAnimation("RunRight", 12, 1, 0);
            AddAnimation("Attack", 12, 2, 0);
            AddAnimation("Dead", 12, 3, 0);
            AddAnimation("Push", 12, 4, 47);
        }

        public void CollisionWithRocket(Rocket rocket)
        {
            if (CollisionBox.Intersects(rocket.CollisionBox))
            {
                if (bossHealth <= 0)
                    playDead = true;
                else
                    bossHealth -= 20;
                rocket.IsVisible = false;
            }
        }
    }
}
