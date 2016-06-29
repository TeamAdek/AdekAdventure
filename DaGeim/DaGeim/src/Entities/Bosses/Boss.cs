using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private List<Rockets> rockets = new List<Rockets>();
        private Texture2D shootTextureLeft;
        private bool playDead;
        private int bossHealth = 180;
        private int attackDelay = 20;
        private int laserDelay = 2;
        public float bossDmg = 15;

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
            position += velocity;
            //rectangle = setCollisionBounds();

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

                    PlayAnimation("Attack");
                    Attack();
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
                Rockets newRocket;

                newRocket = new Rockets(shootTextureLeft);
                newRocket.shootPosition = new Vector2(bossPosition.X - 15, bossPosition.Y + 35);
                newRocket.direction = "left";

                newRocket.isVisible = true;

                // Add rocket to list if they are < 5
                if (rockets.Count < 5)
                    rockets.Add(newRocket);
            }

            if (laserDelay == 0)
                laserDelay = 2;
        }

        public void UpdateAttack()
        {
            foreach (Rockets rocket in rockets)
            {
                //Move rockets according to its direction

                rocket.shootPosition.X = rocket.shootPosition.X - rocket.speed;

                //Set rockets to !visible if they have collided or went off screen

                if (rocket.shootPosition.X < Camera.centre.X - 640 || rocket.shootPosition.X > Camera.centre.X + 640)
                    rocket.isVisible = false;
            }

            //Remove rockets if they are not visible

            for (int i = 0; i < rockets.Count; i++)
            {
                if (!rockets[i].isVisible)
                {
                    rockets.RemoveAt(i);
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

        public override void Draw(SpriteBatch spriteBach)
        {
            foreach (Rockets rocket in rockets)
                rocket.Draw(spriteBach);
            base.Draw(spriteBach);
        }

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack"))
                attacking = false;

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
    }
}
