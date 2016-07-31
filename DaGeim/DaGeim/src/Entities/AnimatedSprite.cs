namespace DaGeim.Entities
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class AnimatedSprite
    {
        /// <summary>
        /// Time
        /// </summary>
        private double timeElapsed; // Stores how much time has passed since the last frame update. (in seconds)
        private double frameTimeSpan; // Determined based on the FPS. Stores how long should 1 frame be. (in seconds)

        /// <summary>
        /// Animation
        /// </summary>
        private Dictionary<string, Rectangle[]> spriteAnimations;
            // Holds all the animations for the given entity/object.

        private int frameIndex; // An indexer, storing information about the current frame of a certian animation.
        protected string currAnimationSet; // Holds the name of the current animation set that needs to be played.

        protected int textureWidth, textureHeight, offSetYPx;
            // Holds offset information for each of the spritesheet texture offsets.

        protected int FramesPerSecond
        {
            set { this.frameTimeSpan = (1.0f/value); }
        }

        protected AnimatedSprite()
        {
            this.spriteAnimations = new Dictionary<string, Rectangle[]>();
        }

        protected void AddAnimation(string name, int frameCount, int offsetY)
        {
            Rectangle[] animationSet = new Rectangle[frameCount];

            for (int i = 0; i < frameCount; i++)
                animationSet[i] = new Rectangle(i*this.textureWidth, offsetY*this.textureHeight, this.textureWidth, this.textureHeight + this.offSetYPx);

            this.spriteAnimations.Add(name, animationSet);
        }

        public virtual void Update(GameTime gameTime)
        {
            this.timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (this.timeElapsed > this.frameTimeSpan)
            {
                this.timeElapsed -= this.frameTimeSpan;

                if (this.frameIndex < this.spriteAnimations[this.currAnimationSet].Length - 1)
                    this.frameIndex++;
                else
                {
                    this.AnimationDone(this.currAnimationSet);
                    this.frameIndex = 0;
                }
            }

        }

        protected virtual void Draw(SpriteBatch spriteBatch, Vector2 entityPosition, Texture2D entityTexture)
        {
            spriteBatch.Draw(entityTexture, entityPosition, this.spriteAnimations[this.currAnimationSet][this.frameIndex], Color.White);
        }

        protected void PlayAnimation(string name)
        {
            if (this.currAnimationSet != name)
            {
                this.currAnimationSet = name;
                this.frameIndex = 0;
            }
        }

        protected abstract void AnimationDone(string animation);
    }
}