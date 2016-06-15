using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DaGeim;

namespace DaGeim.Enemies
{
    using Game.src.Entities;
    public class EnemyGuardian : IEntity
    {
        private Texture2D texture;
        private Vector2 position = new Vector2(164, 384);
        private Vector2 velocity;
        private Rectangle rectangle;
        private Vector2 startPoint = new Vector2(0, 0);
        private bool inPursue = false;
        private string direction = "left";

        private bool hasJumped = false;
        private List<Rockets> rockets;

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

        public Rectangle CollisionBox
        {
            get { return this.rectangle; }
            set { this.rectangle = value; }
        }

        public List<Rockets> Rockets
        {
            get { return this.rockets; }
            set { this.rockets = value; }
        }

        public EnemyGuardian() { }

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

        public void CollisionWithMap(Rectangle tileRectangle, int mapWidth, int mapHeight)
        {
            if (rectangle.TouchTopOf(tileRectangle))
            {
                rectangle.Y = tileRectangle.Y - rectangle.Height;
                velocity.Y = 0f;
                hasJumped = false;
            }

            if (rectangle.TouchLeftOf(tileRectangle))
            {
                position.X = tileRectangle.X - rectangle.Width - 2;
            }

            if (rectangle.TouchRightOf(tileRectangle))
            {
                position.X = tileRectangle.X + tileRectangle.Width + 2;
            }

            if (rectangle.TouchBottomOf(tileRectangle))
                velocity.Y = 1f;


            if (position.X < 0) position.X = 0;
            if (position.X > mapWidth - rectangle.Width) position.X = mapWidth - rectangle.Width;
            if (position.Y < 0) velocity.Y = 1f;
            if (position.Y > mapHeight - rectangle.Height) position.Y = mapHeight - rectangle.Height;
        }

        public void CollisionWithEntity(IEntity entity)
        {
            //TODO enemy collision with other enemy
        }

        public void CollisionWithRocket(Rockets rocket)
        {
            //TODO enemy collision with player rocket
        }

        public void Draw(SpriteBatch spriteBach)
        {
            spriteBach.Draw(texture, rectangle, Color.White);
        }
    }
}