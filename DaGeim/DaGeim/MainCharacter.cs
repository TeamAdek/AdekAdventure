using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaGeim
{
    class MainCharacter : AnimateSprite
    {
        float mySpeed = 100;
        private int FPS = 10;
        private bool moving;
        private bool jumped = false;
        private int jumpHeight = -50;
        private bool attacking = false;
        private bool isDead = false;
        //private int damage = 10;
        protected float health = 100;

        public enum direction { none, left, right };
        protected direction facingDir = direction.none;

        //Setting FPS

        public MainCharacter(Vector2 position) : base(position)
        {
            FramesPerSecond = FPS;
        }

        // Loading content and animations

        public void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("SpriteSheet");
            shootTextureRight = content.Load<Texture2D>("rocketRight");
            shootTextureLeft = content.Load<Texture2D>("rocketLeft");
            AddAnimation(10, 0, "IdleRight");
            AddAnimation(10, 100, "IdleLeft");
            AddAnimation(8, 200, "Right");
            AddAnimation(8, 300, "Left");
            AddAnimation(9, 400, "RunShootRight");
            AddAnimation(9, 500, "RunShootLeft");
            AddAnimation(10, 600, "JumpRight");
            AddAnimation(10, 700, "JumpLeft");
            AddAnimation(8, 800, "JumpMeleeRight");
            AddAnimation(8, 900, "JumpMeleeLeft");
            AddAnimation(4, 1000, "ShootRight");
            AddAnimation(4, 1100, "ShootLeft");
            AddAnimation(8, 1200, "MeleeRight");
            AddAnimation(8, 1300, "MeleeLeft");
            AddAnimation(10, 1400, "SlideRight");
            AddAnimation(10, 1500, "SlideLeft");
            AddAnimation(10, 1600, "DeadRight");
            AddAnimation(10, 1700, "DeadLeft");
        }

        // Handling input and setting the movement via FPS for a smooth movement on multiple devices

        public override void Update(GameTime gameTime)
        {
            playerDirection = Vector2.Zero;

            if (isDead)
            {
                if (facingDir == direction.left)
                    PlayAnimation("DeadLeft");
                else
                    PlayAnimation("DeadRight");
            }
            else
                HandleInput(Keyboard.GetState());

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            playerDirection *= mySpeed;
            playerPosition += (playerDirection * deltaTime);

            base.Update(gameTime);
        }

        private void HandleInput(KeyboardState keyPressed)
        {

            //Movement cotrol with according animations

            if (!attacking && keyPressed.IsKeyDown(Keys.D))
            {
                playerDirection += new Vector2(1, 0);

                if (keyPressed.IsKeyDown(Keys.K) && !jumped)
                {
                    PlayAnimation("RunShootRight");
                    InitializeRocket(playerPosition, "right");
                }
                else if (keyPressed.IsKeyDown(Keys.L) && !jumped)
                    PlayAnimation("SlideRight");
                else if (jumped && keyPressed.IsKeyDown(Keys.J))
                    PlayAnimation("JumpMeleeRight");
                else if (!jumped)
                    PlayAnimation("Right");
                moving = true;
                facingDir = direction.right;
            }
            else if (!attacking && keyPressed.IsKeyDown(Keys.A))
            {
                playerDirection += new Vector2(-1, 0);

                if (keyPressed.IsKeyDown(Keys.K) && !jumped)
                {
                    PlayAnimation("RunShootLeft");
                    InitializeRocket(playerPosition, "left");
                }
                else if (keyPressed.IsKeyDown(Keys.L) && !jumped)
                    PlayAnimation("SlideLeft");
                else if (jumped && keyPressed.IsKeyDown(Keys.J))
                    PlayAnimation("JumpMeleeLeft");
                else if(!jumped)
                    PlayAnimation("Left");
                moving = true;
                facingDir = direction.left;
            }
            else
            {
                moving = false;
            }
               
            // Attacking if player is standing still

            if (!moving)
            {
                if (keyPressed.IsKeyDown(Keys.J))
                {
                    if (facingDir == direction.right)
                    {
                        attacking = true;
                        PlayAnimation("MeleeRight");
                    }

                    if (facingDir == direction.left)
                    {
                        attacking = true;
                        PlayAnimation("MeleeLeft");
                    }
                }

                if (keyPressed.IsKeyDown(Keys.K))
                {
                    if (facingDir == direction.left)
                    {
                        attacking = true;
                        PlayAnimation("ShootLeft");
                        isShooting = true;
                        InitializeRocket(playerPosition, "left");
                    }
                    else
                    {
                        attacking = true;
                        PlayAnimation("ShootRight");
                        isShooting = true;
                        InitializeRocket(playerPosition, "right");
                    }
                }
            }

            // Jumping and Falling
            if (jumped)
            {
                if (playerPosition.Y < 500)
                    playerDirection += new Vector2(0, 1);
                else
                    jumped = false;
            }

            if (!jumped && keyPressed.IsKeyDown(Keys.Space))
            {
                playerDirection += new Vector2(0, jumpHeight);
                jumped = true;
                moving = true;

                if (facingDir == direction.left)
                    PlayAnimation("JumpLeft");
                else
                    PlayAnimation("JumpRight");
            }
            
            // Checking if Idle in order to play Idle animation

            if (!moving && !attacking)
            {
                if (facingDir == direction.left)
                    PlayAnimation("IdleLeft");
                else
                    PlayAnimation("IdleRight");
            }
        }

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Melee") || animation.Contains("Shoot"))
                attacking = false;
        }
    }
}
