using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace DaGeim
{
   public class Player : AnimatedSprite
    {
        public const float VelocityX = 250.0f;
        public const int PLAYER_FPS = 10;
        public int playerHP;
        public float VelocityY = 0.0f;
        private bool sliding = false;
        private bool jumped = false;
        public bool grounded = false;
        private float jumpHeight = 400;
        private float gravity = 15f;
        private float rocketDelay = 2;
        public Rectangle collisionBox;
        public Vector2 stepAmount;
        private Viewport viewport;
        private List<Rockets> rockets = new List<Rockets>();

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Get and set collision box
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public Rectangle getCB()
        {
            return collisionBox;
        }

        public void setCB(Rectangle b)
        {
            collisionBox = b;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Get player position
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public Vector2 getPosition()
        {
            return playerPosition;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Player Constructor
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        

        public Player(Vector2 position) : base(position)
        {
            FramesPerSecond = PLAYER_FPS;
            playerHP = 320;

            loadAnimations();
            PlayAnimation("IdleRight");
            currentDirection = PlayerDirection.Right;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Load images
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public void LoadContent(ContentManager content)
        {
            spriteTexture = content.Load<Texture2D>("PlayerAnimation");
            shootTextureRight = content.Load<Texture2D>("rocketRight");
            shootTextureLeft = content.Load<Texture2D>("rocketLeft");
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Update Game
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public override void Update(GameTime gameTime)
        {
            if (jumped)
                spriteDirection = new Vector2(0f, 1f);
            else
                spriteDirection = Vector2.Zero;



            HandleInput(Keyboard.GetState());

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (sliding)
            {
                if (currentDirection == PlayerDirection.Left)
                    spriteDirection += new Vector2(-1f, 0f);
                else
                    spriteDirection += new Vector2(1f, 0f);

                //            VelocityY -= 52;
            }

            if (!grounded && !jumped)
            {
                spriteDirection += new Vector2(0, 3.0f);
            }
            VelocityY += gravity * -1;
            spriteDirection *= new Vector2(VelocityX, -VelocityY);
            playerPosition += (spriteDirection * deltaTime);

            stepAmount = spriteDirection * deltaTime;

            setCollisionBounds();
            base.Update(gameTime);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Draw rockets to screen overriding/updating the AnimateSprite.Draw method
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Rockets rocket in rockets)
                rocket.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Handle input from user
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public void HandleInput(KeyboardState Keyboard)
        {
            // When our char is slding, we should'nt be able to control him
            if (!sliding)
            {
                /// LEFT ARROW KEY 
                if (Keyboard.IsKeyDown(Keys.Left)) // Left Side Animations
                {
                    if (jumped)
                    {
                        if (Keyboard.IsKeyDown(Keys.X))
                        {
                            attacking = true;
                            PlayAnimation("MeleeLeft");
                        }
                        else
                            PlayAnimation("JumpLeft");
                    }

                    spriteDirection += new Vector2(-1f, 0f);
                    if (Keyboard.IsKeyDown(Keys.Down) && !jumped) /// SLIDE 
                    {
                        sliding = true;
                        PlayAnimation("SlideLeft");
                    }
                    else if (Keyboard.IsKeyDown(Keys.Space) && !jumped) /// SHOOT
                    {
                        attacking = true;
                        PlayAnimation("AttackLeft");
                        Shoot();
                    }
                    else if (Keyboard.IsKeyDown(Keys.Up) && !jumped) /// JUMP
                    {
                        jumped = true;
                        VelocityY = jumpHeight;
                        spriteDirection += new Vector2(0f, 1f);

                        if (Keyboard.IsKeyDown(Keys.X))
                        {
                            attacking = true;
                            PlayAnimation("MeleeLeft");
                        }
                        else
                            PlayAnimation("JumpLeft");
                    }
                    else if (!jumped) /// RUN
                        PlayAnimation("RunLeft");
                    currentDirection = PlayerDirection.Left;
                }

                /// RIGHT ARROW KEY 
                else if (Keyboard.IsKeyDown(Keys.Right))
                {
                    if (jumped)
                    {
                        if (Keyboard.IsKeyDown(Keys.X))
                        {
                            attacking = true;
                            PlayAnimation("MeleeRight");
                        }
                        else
                            PlayAnimation("JumpRight");
                    }
                    spriteDirection += new Vector2(1f, 0f);
                    if (Keyboard.IsKeyDown(Keys.Down) && !jumped) /// SLIDE 
                    {
                        sliding = true;
                        PlayAnimation("SlideRight");
                    }
                    else if (Keyboard.IsKeyDown(Keys.Space) && !jumped) /// SHOOT
                    {
                        attacking = true;
                        PlayAnimation("AttackRight");
                        Shoot();
                    }
                    else if (Keyboard.IsKeyDown(Keys.Up) && !jumped) /// JUMP 
                    {
                        jumped = true;
                        spriteDirection += new Vector2(0f, 1f);
                        VelocityY = jumpHeight;

                        if (Keyboard.IsKeyDown(Keys.X))
                        {
                            attacking = true;
                            PlayAnimation("MeleeRight");
                        }
                        else
                            PlayAnimation("JumpRight");
                    }
                    else if (!jumped) /// RUN
                        PlayAnimation("RunRight");

                    currentDirection = PlayerDirection.Right;
                }

                /// SPACE
                else if (Keyboard.IsKeyDown(Keys.Space))
                {
                    attacking = true;
                    if (currentDirection == PlayerDirection.Left)
                    {
                        currentDirection = PlayerDirection.Left;
                        PlayAnimation("ShootLeft");
                        Shoot();
                    }
                    else if (currentDirection == PlayerDirection.Right)
                    {
                        currentDirection = PlayerDirection.Right;
                        PlayAnimation("ShootRight");
                        Shoot();
                    }
                }
                /// UP ARROW
                else if (Keyboard.IsKeyDown(Keys.Up) && !jumped)
                {
                    jumped = true;
                    spriteDirection += new Vector2(0f, 1f);
                    VelocityY = jumpHeight;

                    if (currentDirection == PlayerDirection.Left)
                        PlayAnimation("JumpLeft");
                    else
                        PlayAnimation("JumpRight");
                }
                else if (Keyboard.IsKeyDown(Keys.X))
                {
                    attacking = true;
                    if (currentDirection == PlayerDirection.Left)
                        PlayAnimation("IdleMeleeLeft");
                    else
                        PlayAnimation("IdleMeleeRight");
                }
                /// NO KEY PRESSED
                else
                {
                    if (!jumped)
                    {
                        if (currentDirection == PlayerDirection.Left)
                            PlayAnimation("IdleLeft");
                        if (currentDirection == PlayerDirection.Right)
                            PlayAnimation("IdleRight");
                    }
                }

                UpdateRocket();
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Check if specific animation has ended
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack") || animation.Contains("Shoot"))
                attacking = false;

            if (animation.Contains("Slide"))
                sliding = false;
        }


        public void Collision(Rectangle otherObjectRect)
        {
            if (!collisionBox.TouchTopOf(otherObjectRect) && !jumped)
                grounded = false;
            if (collisionBox.TouchTopOf(otherObjectRect))
            {
                if (sliding)
                    playerPosition.Y = otherObjectRect.Y - collisionBox.Height - 25;
                else
                    playerPosition.Y = otherObjectRect.Y - collisionBox.Height - 9;

                VelocityY = 0.0f;
                jumped = false;
                grounded = true;
            }
            if (collisionBox.TouchLeftOf(otherObjectRect) && currentDirection == PlayerDirection.Right)
            {
                sliding = false;
                playerPosition.X -= stepAmount.X;
            }
            if (collisionBox.TouchRightOf(otherObjectRect) && currentDirection == PlayerDirection.Left)
            {
                sliding = false;
                playerPosition.X -= stepAmount.X;
            }
            if (collisionBox.TouchBottomOf(otherObjectRect))
                VelocityY = -1.0f;


        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Set all collision bounds
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        private void setCollisionBounds()
        {
            int xOffset, yOffset, width, height;

            if (currentAnimation.Equals("IdleLeft"))
            {
                xOffset = 24;
                yOffset = 10;
                width = 47;
                height = 83;
            }
            else if (currentAnimation.Equals("IdleRight"))
            {
                xOffset = 30;
                yOffset = 10;
                width = 47;
                height = 83;
            }
            else if (currentAnimation.Equals("RunRight"))
            {
                xOffset = 10;
                yOffset = 10;
                width = 57;
                height = 83;
            }
            else if (currentAnimation.Equals("RunLeft"))
            {
                xOffset = 33;
                yOffset = 10;
                width = 57;
                height = 83;
            }
            else if (currentAnimation.Equals("AttackRight"))
            {
                xOffset = 14;
                yOffset = 10;
                width = 70;
                height = 83;
            }
            else if (currentAnimation.Equals("AttackLeft"))
            {
                xOffset = 16;
                yOffset = 10;
                width = 70;
                height = 83;
            }
            else if (currentAnimation.Equals("JumpRight"))
            {
                xOffset = 14;
                yOffset = 10;
                width = 66;
                height = 83;
            }
            else if (currentAnimation.Equals("JumpLeft"))
            {
                xOffset = 20;
                yOffset = 10;
                width = 66;
                height = 83;
            }
            else if (currentAnimation.Equals("MeleeRight"))
            {
                xOffset = 13;
                yOffset = 10;
                width = 75;
                height = 88;
            }
            else if (currentAnimation.Equals("MeleeLeft"))
            {
                xOffset = 12;
                yOffset = 10;
                width = 75;
                height = 88;
            }
            else if (currentAnimation.Equals("ShootRight"))
            {
                xOffset = 21;
                yOffset = 10;
                width = 57;
                height = 83;
            }
            else if (currentAnimation.Equals("ShootLeft"))
            {
                xOffset = 22;
                yOffset = 10;
                width = 56;
                height = 83;
            }
            else if (currentAnimation.Equals("IdleMeleeRight"))
            {
                xOffset = 15;
                yOffset = 10;
                width = 70;
                height = 83;
            }
            else if (currentAnimation.Equals("IdleMeleeLeft"))
            {
                xOffset = 18;
                yOffset = 10;
                width = 70;
                height = 83;
            }
            else if (currentAnimation.Equals("SlideRight"))
            {
                xOffset = 8;
                yOffset = 30;
                width = 68;
                height = 65;
            }
            else if (currentAnimation.Equals("SlideLeft"))
            {
                xOffset = 23;
                yOffset = 30;
                width = 68;
                height = 65;
            }
            else
            {
                xOffset = 0;
                yOffset = 0;
                width = 100;
                height = 100;
            }

            Rectangle output = new Rectangle(((int)playerPosition.X + xOffset), ((int)playerPosition.Y + yOffset),
                width, height);
            collisionBox = output;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Load all animations to dictionary
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        private void loadAnimations()
        {
            AddAnimation("IdleRight", 10, 0);
            AddAnimation("IdleLeft", 10, 100);
            AddAnimation("RunRight", 8, 200);
            AddAnimation("RunLeft", 8, 300);
            AddAnimation("AttackRight", 9, 400);
            AddAnimation("AttackLeft", 9, 500);
            AddAnimation("JumpRight", 10, 600);
            AddAnimation("JumpLeft", 10, 700);
            AddAnimation("MeleeRight", 8, 800);
            AddAnimation("MeleeLeft", 8, 900);
            AddAnimation("ShootRight", 4, 1000);
            AddAnimation("ShootLeft", 4, 1100);
            AddAnimation("IdleMeleeRight", 8, 1200);
            AddAnimation("IdleMeleeLeft", 8, 1300);
            AddAnimation("SlideRight", 10, 1400);
            AddAnimation("SlideLeft", 10, 1500);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Setting up rockets and adding to list
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public void Shoot()
        {
            if (rocketDelay >= 0)
                rocketDelay--;

            if (rocketDelay <= 0)
            {
                Rockets newRocket;

                // Set the new rockets direction and offsets

                if (currentDirection == PlayerDirection.Left)
                {
                    newRocket = new Rockets(shootTextureLeft);
                    newRocket.shootPosition = new Vector2(playerPosition.X - 15, playerPosition.Y + 35);
                    newRocket.direction = "left";
                }
                else
                {
                    newRocket = new Rockets(shootTextureRight);
                    newRocket.shootPosition = new Vector2(playerPosition.X + 80, playerPosition.Y + 35);
                    newRocket.direction = "right";
                }

                newRocket.isVisible = true; // set current rocket's visibility to true

                // Add rocket to list if they are < 20

                if (rockets.Count < 20)
                    rockets.Add(newRocket);
            }

            //Reset rocket delay timer

            if (rocketDelay == 0)
                rocketDelay = 20;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Update rocket positions and remove collided rockets or those off screen
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public void UpdateRocket()
        {
            foreach (Rockets rocket in rockets)
            {
                //Move rockets according to its direction

                if (rocket.direction == "left")
                    rocket.shootPosition.X = rocket.shootPosition.X - rocket.speed;
                else
                    rocket.shootPosition.X = rocket.shootPosition.X + rocket.speed;

                //Set rockets to !visible if they have collided or went off screen

                if(rocket.shootPosition.X < Camera.centre.X - 640 || rocket.shootPosition.X > Camera.centre.X + 640)
                    rocket.isVisible = false;
            }

            //Remove rockets if they are not visible

            for (int i = 0; i < rockets.Count; i++)
            {
                if (!rockets[i].isVisible)
                {
                    rockets.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}



