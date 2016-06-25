using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.src.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DaGeim.src.Collectable
{
    class HealthRestore : IEntity
    {
        protected Texture2D spriteTexture;
        //private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private Rectangle rectangle;
        private Vector2 startPoint = new Vector2(0, 0);
        private const int ENEMY_FPS = 15;
        public bool dead = false;
        

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

        public List<Rockets> Rockets { get; set; }


        public HealthRestore(Vector2 position)
        {
            this.position = position;
            this.startPoint = position;
            //loadAnimations();
            
        }

        public void Load(ContentManager Content)
        {
            //spriteTexture = Content.Load<Texture2D>("heart");
            spriteTexture = Content.Load<Texture2D>("pow");
            spriteTexture = Content.Load<Texture2D>("Corazon");
        }

        public void CollisionWithPlayer(IEntity entity)
        {
            if (this.CollisionBox.Intersects(entity.CollisionBox))
            {
                Position=new Vector2(-1000,-1000);
            }


        }

        public void Update(GameTime gameTime)
        {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Position += velocity;
            // rectangle = setCollisionBounds();
            //rectangle = setRectangle((int)this.Position.X,(int) this.Position.Y, 100, 200); 
            rectangle = setRectangle(0,0, 30, 30); 
            
            

            if (velocity.Y < 10)
                velocity.Y += 0.4f;

            

            //base.Update(gameTime);
        }

        
        public void CollisionWithMap(Rectangle tileRectangle, int mapWidth, int mapHeight)
        {
            if (rectangle.TouchTopOf(tileRectangle))
            {
                rectangle.Y = tileRectangle.Y - rectangle.Height;
                velocity.Y = 0f;
                //hasJumped = false;
            }

            if (rectangle.TouchLeftOf(tileRectangle))
            {
                Position = new Vector2( tileRectangle.X - rectangle.Width - 2, Position.Y);
            }

            if (rectangle.TouchRightOf(tileRectangle))
            {
                Position = new Vector2(tileRectangle.X + tileRectangle.Width + 2, Position.Y);
            }

            if (rectangle.TouchBottomOf(tileRectangle))
                velocity.Y = 1f;


            if (Position.X < 0) Position = new Vector2(0, position.Y);
            if (Position.X > mapWidth - rectangle.Width) Position = new Vector2( mapWidth - rectangle.Width, position.Y);
            if (Position.Y < 0) velocity.Y = 1f;
            if (Position.Y > mapHeight - rectangle.Height) Position = new Vector2(position.X,mapHeight - rectangle.Height);
        }

        //private Rectangle setCollisionBounds()
        //{
        //    Rectangle output = new Rectangle();
        //    switch (currentAnimation)
        //    {
        //        case "IdleLeft": output = setRectangle(6, 0, 64, 90); break;
        //        case "IdleRight": output = setRectangle(0, 0, 64, 90); break;
        //        case "WalkLeft": output = setRectangle(3, 0, 59, 90); break;
        //        case "WalkRight": output = setRectangle(8, 0, 59, 90); break;
        //        default: output = setRectangle(0, 0, 70, 90); break; ;
        //    }

        //    return output;
        //}

        private Rectangle setRectangle(int x, int y, int w, int h)
        {
            return new Rectangle((int)Position.X + x, (int)Position.Y + y, w, h);
        }

        //private void loadAnimations()
        //{
        //    AddAnimation("IdleLeft", 0);
        //    AddAnimation("IdleRight", 1);
        //    AddAnimation("WalkLeft", 2);
        //    AddAnimation("WalkRight", 3);
        //}

        //public override void AnimationDone(string animation)
        //{
        //    if (animation.Contains("Attack") || animation.Contains("Shoot"))
        //        attacking = false;
        //}

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D t = spriteTexture;
            Vector2 v = Position;
            //Rectangle src = spriteAnimations[currentAnimation][frameIndex];
            spriteBatch.Draw(t, v, Color.White);
            //base.Draw(spriteBach);
        }

        public void CollisionWithEntity(IEntity entity)
        {

        }

        public void CollisionWithRocket(Rockets rocket, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
