using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotBoy.Entities.Bosses
{
    public abstract class AnimatedBoss
    {
        // Boss Details
        protected Vector2 bossPosition;
        protected Texture2D spriteTexture;
        protected bool dead = false;

        // Animations
        private int frameIndex;
        private double timeElapsed;
        private double timeToUpdate;
        protected string currentAnimation;
        private Dictionary<string, Rectangle[]> spriteAnimations = new Dictionary<string, Rectangle[]>();

        public enum Direction { None, Left, Right }
        protected Direction currentDirection = Direction.None;

        public int FramesPerSecond
        {
            set { timeToUpdate = (1f / value); }
        }

        public AnimatedBoss(Vector2 position)
        {
            bossPosition = position;
        }

        public void AddAnimation(string name, int frameCount, int yPos, int yOffset)
        {
            Rectangle[] animationSet = new Rectangle[frameCount];

            for (int i = 0; i < frameCount; i++)
                animationSet[i] = new Rectangle((i) * 128, yPos * 128, 128, 128 + yOffset);

            spriteAnimations.Add(name, animationSet);
        }

        public virtual void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;

                if (frameIndex < (spriteAnimations[currentAnimation].Length - 1))
                    frameIndex++;
                else
                {
                    AnimationDone(currentAnimation);
                    frameIndex = 0;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!dead)
                spriteBatch.Draw(spriteTexture, bossPosition, spriteAnimations[currentAnimation][frameIndex], Color.White);
        }

        public void PlayAnimation(string name)
        {
            if (currentAnimation != name)
            {
                if (name.Contains("Push"))
                    bossPosition.Y -= 47;
                currentAnimation = name;
                frameIndex = 0;
            }
        }

        public abstract void AnimationDone(string animation);
    }
}
