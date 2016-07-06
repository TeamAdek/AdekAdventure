using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.src.Entities
{

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
            set { frameTimeSpan = (1.0f/value); }
        }

        protected AnimatedSprite()
        {
            spriteAnimations = new Dictionary<string, Rectangle[]>();
        }

        protected void AddAnimation(string name, int frameCount, int offsetY)
        {
            Rectangle[] animationSet = new Rectangle[frameCount];

            for (int i = 0; i < frameCount; i++)
                animationSet[i] = new Rectangle(i*textureWidth, offsetY*textureHeight, textureWidth, textureHeight + offSetYPx);

            spriteAnimations.Add(name, animationSet);
        }

        public virtual void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeElapsed > frameTimeSpan)
            {
                timeElapsed -= frameTimeSpan;

                if (frameIndex < spriteAnimations[currAnimationSet].Length - 1)
                    frameIndex++;
                else
                {
                    AnimationDone(currAnimationSet);
                    frameIndex = 0;
                }
            }

        }

        protected virtual void Draw(SpriteBatch spriteBatch, Vector2 entityPosition, Texture2D entityTexture)
        {
            spriteBatch.Draw(entityTexture, entityPosition, spriteAnimations[currAnimationSet][frameIndex], Color.White);
        }

        protected void PlayAnimation(string name)
        {
            if (currAnimationSet != name)
            {
                currAnimationSet = name;
                frameIndex = 0;
            }
        }

        protected abstract void AnimationDone(string animation);
    }
}