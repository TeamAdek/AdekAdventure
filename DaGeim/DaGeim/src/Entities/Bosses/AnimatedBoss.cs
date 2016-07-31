namespace DaGeim.Entities.Bosses
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

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
            set { this.timeToUpdate = (1f / value); }
        }

        public AnimatedBoss(Vector2 position)
        {
            this.bossPosition = position;
        }

        public void AddAnimation(string name, int frameCount, int yPos, int yOffset)
        {
            Rectangle[] animationSet = new Rectangle[frameCount];

            for (int i = 0; i < frameCount; i++)
                animationSet[i] = new Rectangle((i) * 128, yPos * 128, 128, 128 + yOffset);

            this.spriteAnimations.Add(name, animationSet);
        }

        public virtual void Update(GameTime gameTime)
        {
            this.timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (this.timeElapsed > this.timeToUpdate)
            {
                this.timeElapsed -= this.timeToUpdate;

                if (this.frameIndex < (this.spriteAnimations[this.currentAnimation].Length - 1))
                    this.frameIndex++;
                else
                {
                    this.AnimationDone(this.currentAnimation);
                    this.frameIndex = 0;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!this.dead)
                spriteBatch.Draw(this.spriteTexture, this.bossPosition, this.spriteAnimations[this.currentAnimation][this.frameIndex], Color.White);
        }

        public void PlayAnimation(string name)
        {
            if (this.currentAnimation != name)
            {
                if (name.Contains("Push"))
                    this.bossPosition.Y -= 47;
                this.currentAnimation = name;
                this.frameIndex = 0;
            }
        }

        public abstract void AnimationDone(string animation);
    }
}
