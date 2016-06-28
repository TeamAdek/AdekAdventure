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
        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += velocity;
            //rectangle = setCollisionBounds();
            Patrol();
            base.Update(gameTime);
        }

        public void Patrol()
        {
            if (attacking == false)
            {
                if ((direction == "left") && (bossPosition.X > startPoint.X - patrolRange))
                {
                    bossPosition.X-=2;
                    PlayAnimation("RunRight");

                }

                if ((direction == "left") && (bossPosition.X <= startPoint.X - patrolRange))
                {
                    PlayAnimation("RunRight");
                    direction = "right";
                }

                if ((direction == "right") && (bossPosition.X < startPoint.X + patrolRange))
                {
                    PlayAnimation("RunLeft");
                    bossPosition.X+=2;
                }

                if ((direction == "right") && (bossPosition.X >= startPoint.X + patrolRange))
                {
                    PlayAnimation("RunLeft");
                    direction = "left";
                }
            }
        }

        public override void Draw(SpriteBatch spriteBach)
        {
            base.Draw(spriteBach);
        }

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack") || animation.Contains("Shoot"))
                attacking = false;
        }

        private void loadAnimations()
        {
            AddAnimation("RunLeft", 12, 0);
            AddAnimation("RunRight", 12, 1);
        }
    }
}
