using System;
using System.Collections.Generic;
using DaGeim.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace DaGeim.Enemies
{
    using Game.src.Entities;
    public class EnemyGuardian : AnimatedEnemy, IEntity, IPatrolable
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private Rectangle rectangle;
        private Vector2 startPoint = new Vector2(0, 0);
        private bool inPursue = false;
        private string direction = "left";
        private int patrolRange;
        private const int ENEMY_FPS = 15;
        public bool dead = false;
        private int enemyHealth = 90;

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

        public int PatrolRange
        {
            get { return patrolRange; }
            set { patrolRange = value; }
        }

        public List<Rockets> Rockets
        {
            get { return this.rockets; }
            set { this.rockets = value; }
        }

        public EnemyGuardian(Vector2 position, int range) : base(position)
        {
            this.position = position;
            this.startPoint = position;
            patrolRange = range;
            FramesPerSecond = ENEMY_FPS;

            loadAnimations();
            PlayAnimation("IdleLeft");
            currentDirection = Direction.Left;
        }

        public void Load(ContentManager Content)
        {
            spriteTexture = Content.Load<Texture2D>("SpriteSheetEnemy");
        }
        public void Update(GameTime gameTime, Vector2 playerPosition)
        {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            enemyPosition += velocity;
            rectangle = setCollisionBounds();
            inPursue = false;

            if (velocity.Y < 10)
                velocity.Y += 0.4f;

            // detect player and pursue him
            if ((playerPosition.X < enemyPosition.X) &&
                (enemyPosition.X - playerPosition.X < 155) &&
                (enemyPosition.X > startPoint.X - 35) &&
                (Math.Abs(position.Y - playerPosition.Y) < 150)
                )
            {
                inPursue = true;
                direction = "left";
                PlayAnimation("WalkLeft");
                enemyPosition.X -= 1;
            }

            if ((playerPosition.X > enemyPosition.X) &&
                (playerPosition.X - enemyPosition.X < 155) &&
                (enemyPosition.X < startPoint.X + 35) &&
                (Math.Abs(position.Y - playerPosition.Y) < 150)
                )
            {
                inPursue = true;
                direction = "right";
                PlayAnimation("WalkRight");
                enemyPosition.X += 1;
            }

            // patrol
            Patrol();
            
            base.Update(gameTime);
        }

        public void Patrol()
        {
            if (inPursue == false)
            {
                if ((direction == "left") && (enemyPosition.X > startPoint.X - patrolRange))
                {
                    enemyPosition.X--;
                    PlayAnimation("WalkLeft");

                }

                if ((direction == "left") && (enemyPosition.X <= startPoint.X - patrolRange))
                {
                    PlayAnimation("WalkRight");

                    direction = "right";
                }

                if ((direction == "right") && (enemyPosition.X < startPoint.X + patrolRange))
                {
                    PlayAnimation("WalkRight");

                    enemyPosition.X++;
                }

                if ((direction == "right") && (enemyPosition.X >= startPoint.X + patrolRange))
                {
                    PlayAnimation("WalkLeft");
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
                enemyPosition.X = tileRectangle.X - rectangle.Width - 2;
            }

            if (rectangle.TouchRightOf(tileRectangle))
            {
                enemyPosition.X = tileRectangle.X + tileRectangle.Width + 2;
            }

            if (rectangle.TouchBottomOf(tileRectangle))
                velocity.Y = 1f;


            if (enemyPosition.X < 0) enemyPosition.X = 0;
            if (enemyPosition.X > mapWidth - rectangle.Width) enemyPosition.X = mapWidth - rectangle.Width;
            if (enemyPosition.Y < 0) velocity.Y = 1f;
            if (enemyPosition.Y > mapHeight - rectangle.Height) enemyPosition.Y = mapHeight - rectangle.Height;
        }

        private Rectangle setCollisionBounds()
        {
            Rectangle output = new Rectangle();
            switch (currentAnimation)
            {
                case "IdleLeft": output = setRectangle(6, 0, 64, 90); break;
                case "IdleRight": output = setRectangle(0, 0, 64, 90); break;
                case "WalkLeft": output = setRectangle(3, 0, 59, 90); break;
                case "WalkRight": output = setRectangle(8, 0, 59, 90); break;
                default: output = setRectangle(0, 0, 70, 90); break; ;
            }

            return output;
        }

        private Rectangle setRectangle(int x, int y, int w, int h)
        {
            return new Rectangle((int)enemyPosition.X + x, (int)enemyPosition.Y + y, w, h);
        }

        private void loadAnimations()
        {
            AddAnimation("IdleLeft", 0);
            AddAnimation("IdleRight", 1);
            AddAnimation("WalkLeft", 2);
            AddAnimation("WalkRight", 3);
        }

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack") || animation.Contains("Shoot"))
                attacking = false;
        }

        public override void Draw(SpriteBatch spriteBach)
        {
            base.Draw(spriteBach);
        }

        public void CollisionWithEntity(IEntity entity)
        {

        }

        public void CollisionWithRocket(Rockets rocket, Player player)
        {
            if (CollisionBox.Intersects(rocket.getCollisionBox()))
            {
                if (enemyHealth <= 0)
                {
                    player.playerScore += 50;
                    dead = true;
                }
                else
                    enemyHealth -= 50;

                rocket.isVisible = false;
            }

        }
    }
}