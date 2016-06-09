using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaGeim
{
    abstract class AnimateSprite
    {
        protected Texture2D playerTexture;
        protected Vector2 playerPosition;
        protected Texture2D shootTextureRight;
        protected Texture2D shootTextureLeft;
        protected Vector2 shootPosition;
        private int frameIndex;

        protected bool isShooting = false;
        protected bool shotCollision = false;

        private double timeElapsed;
        private double timeToUpdate;

        protected Vector2 playerDirection = Vector2.Zero;
        private Dictionary<string, Rectangle[]> spriteSheet = new Dictionary<string, Rectangle[]>();
        protected string currAnimation;

        private int shoot_X_offset = 80;
        private int shoot_Y_offset = 35;
        private bool isShootRight = true;

        public int FramesPerSecond
        {
            set { timeToUpdate = (1f / value); }
        }

        public AnimateSprite(Vector2 position)
        {
            playerPosition = position;
        }

        public void AddAnimation(int frames, int yPos, string name)
        {
            Rectangle[] currRowAnimation = new Rectangle[frames];

            for (int i = 0; i < frames; i++)
                currRowAnimation[i] = new Rectangle(i* 100, yPos, 100, 100);

            spriteSheet.Add(name, currRowAnimation);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!shotCollision && isShootRight)
                shootPosition.X += 3;
            else if (!shotCollision && !isShootRight)
                shootPosition.X -= 3;
            else
                isShooting = false;

            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;
                if (frameIndex < spriteSheet[currAnimation].Length - 1)
                    frameIndex++;
                else
                {
                    AnimationDone(currAnimation);
                    frameIndex = 0;
                }
            }

        }

        public void InitializeRocket(Vector2 startPos, string direction)
        {
            if (direction == "right")
            {
                shoot_X_offset = 80;
                shoot_Y_offset = 35;
                isShootRight = true;
            }
            else
            {
                shoot_X_offset = -15;
                shoot_Y_offset = 35;
                isShootRight = false;
            }

            shootPosition.X = startPos.X + shoot_X_offset;
            shootPosition.Y = startPos.Y + shoot_Y_offset;
        }

        public void Draw(SpriteBatch spriteBach)
        {
            if (isShooting && isShootRight)
                spriteBach.Draw(shootTextureRight, shootPosition, Color.White);
            else if (isShooting && !isShootRight)
                spriteBach.Draw(shootTextureLeft, shootPosition, Color.White);
            spriteBach.Draw(playerTexture, playerPosition, spriteSheet[currAnimation][frameIndex], Color.White);
        }

        public void PlayAnimation(string name)
        {
            if (currAnimation != name)
            {
                currAnimation = name;
                frameIndex = 0;
            }
        }

        public abstract void AnimationDone(string animation);

    }
}
