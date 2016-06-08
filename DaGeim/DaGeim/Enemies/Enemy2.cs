using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DaGeim.Enemies
{
    public class Enemy2
    {
        private Texture2D texture;
        private Vector2 position = new Vector2(164, 384);
        private Vector2 velocity;
        private Rectangle rectangle;

        private bool hasJumped = false;

        public Vector2 Position
        {
            get { return position; }
            set { this.position = value; }
        }

        public Enemy2() { }

        public void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Upgraded_Robot_Sprite");
        }
        public void Update(GameTime gameTime)
        {
            position += velocity;
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            Input(gameTime);

            if (velocity.Y < 10)
                velocity.Y += 0.4f;



        }
        private void Input(GameTime gameTime)
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.Right))
            //    velocity.X = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 4;
            //else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            //    velocity.X = -(float)gameTime.ElapsedGameTime.TotalMilliseconds / 4;
            //else
            //    velocity.X = 0f;

            //if (Keyboard.GetState().IsKeyDown(Keys.Up) && hasJumped == false)
            //{
            //    position.Y -= 5f;
            //    velocity.Y = -9f;
            //    hasJumped = true;
            //}
        }

        public void Collision(Rectangle newRectangle, int xOffset, int yOffset)
        {
            if (rectangle.TouchTopOf(newRectangle))
            {
                rectangle.Y = newRectangle.Y - rectangle.Height;
                velocity.Y = 0f;
                hasJumped = false;
            }

            if (rectangle.TouchLeftOf(newRectangle))
            {
                position.X = newRectangle.X - rectangle.Width - 2;
            }

            if (rectangle.TouchRightOf(newRectangle))
            {
                position.X = newRectangle.X + newRectangle.Width + 2;
            }

            if (rectangle.TouchBottomOf(newRectangle))
                velocity.Y = 1f;


            if (position.X < 0) position.X = 0;
            if (position.X > xOffset - rectangle.Width) position.X = xOffset - rectangle.Width;
            if (position.Y < 0) velocity.Y = 1f;
            if (position.Y > yOffset - rectangle.Height) position.Y = yOffset - rectangle.Height;
        }

        public void Draw(SpriteBatch spriteBach)
        {
            spriteBach.Draw(texture, rectangle, Color.White);
        }
    }
}