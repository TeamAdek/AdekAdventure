using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DaGeim;

namespace DaGeim.Enemies
{
    public class Enemy2
    {
        private Texture2D texture;
        private Vector2 position = new Vector2(164, 384);
        private Vector2 velocity;
        private Rectangle rectangle;
        private Vector2 startPoint = new Vector2(0, 0);
        private bool inPursue = false;
        private string direction = "left";

        private bool hasJumped = false;

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public Vector2 StartPoint
        {
            get { return this.startPoint; }
            set { this.startPoint = value; }
        }

        public Enemy2() { }

        public void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("Upgraded_Robot_Sprite");
            //texture = Content.Load<Texture2D>("enemy_with_sword");
        }
        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            position += velocity;
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            inPursue = false;

            if (velocity.Y < 10)
                velocity.Y += 0.4f;

            // detect player and pursue him
            if ((playerPosition.X < position.X) &&
                (position.X - playerPosition.X < 155) &&
                (position.X > startPoint.X - 35) &&
                (Math.Abs(position.Y - playerPosition.Y) < 150)
                )
            {
                inPursue = true;
                direction = "left";
                position.X -= 1;
            }

            if ((playerPosition.X > position.X) &&
                (playerPosition.X - position.X < 155) &&
                (position.X < startPoint.X + 35) &&
                (Math.Abs(position.Y - playerPosition.Y) < 150)
                )
            {
                inPursue = true;
                direction = "right";
                position.X += 1;
            }

            // patrol
            if (inPursue == false)
            {
                if ((direction == "left") && (position.X > startPoint.X - 30))
                {
                    position.X--;
                }

                if ((direction == "left") && (position.X <= startPoint.X - 30))
                {
                    direction = "right";
                }

                if ((direction == "right") && (position.X < startPoint.X + 30))
                {
                    position.X++;
                }

                if ((direction == "right") && (position.X >= startPoint.X + 30))
                {
                    direction = "left";
                }
            }
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