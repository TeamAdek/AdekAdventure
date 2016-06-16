using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


    public abstract class AnimatedEnemy
    {
        protected Texture2D spriteTexture;

        protected Vector2 enemyPosition;
        protected Vector2 spriteDirection = Vector2.Zero;
        private Rectangle[] textureArea;
        private Vector2 shootPosition;
        protected Texture2D shootTextureRight;
        protected Texture2D shootTextureLeft;
        private bool shotCollision = false;

        private int frameIndex;
        private double timeElapsed;
        private double timeToUpdate;
        protected string currentAnimation;
        protected bool attacking = false;

        private Dictionary<string, Rectangle[]> spriteAnimations = new Dictionary<string, Rectangle[]>();

        public enum Direction {None, Left, Right}
        protected Direction currentDirection = Direction.None;

        public int FramesPerSecond
        {
            set { timeToUpdate = (1f/value); }
        }

        public AnimatedEnemy(Vector2 position)
        {
            enemyPosition = position;
        }

        public void AddAnimation(string name, int yPos)
        {
            Rectangle[] animationSet = new Rectangle[10];

            for (int i = 0; i < 10; i++)
                animationSet[i] = new Rectangle((i) * 70, yPos * 90, 70, 90);      

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
            Texture2D t = spriteTexture;
            Vector2 v = enemyPosition;
            Rectangle src = spriteAnimations[currentAnimation][frameIndex];
            spriteBatch.Draw(t, v, src, Color.White);
            
        }

        public void PlayAnimation(string name)
        {
            if (currentAnimation != name)
            {
                currentAnimation = name;
                frameIndex = 0;
            }
        }
        public abstract void AnimationDone(string animation);
    }
