using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using DaGeim.Interfaces;

namespace DaGeim
{
    using System;
    using Game.src.Entities;

    public class Player : AnimatedSprite, IEntity, ICollecting, IJumpboostable
    {
        public const float VelocityX = 250.0f;
        public const int PLAYER_FPS = 10;
        public int playerHP;
        public bool grounded = false;
        public float VelocityY = 0.0f;
        private bool sliding = false;
        private bool jumped = false;
        private bool slideCD = false;
        private double slideCDTimer = 0.0f;
        private float jumpHeight = 400;
        private float gravity = 15f;
        private float rocketDelay = 2;
        public Rectangle collisionBox;
        public Vector2 stepAmount;
        private Viewport viewport;
        private List<Rockets> rockets = new List<Rockets>();
        private int xOffset, yOffset, width, height;
        private List<SoundEffect> sounds = new List<SoundEffect>();
        private int jumpBoostJumpBoostTimer = 0;

        public int JumpBoostTimer
        {
            get { return jumpBoostJumpBoostTimer; }
            set { jumpBoostJumpBoostTimer = value; }
        }

        /*---------------------------------------*/
        // REPLACE THIS ONE
        public int playerScore = 0;
        /*--------------------------------------*/

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Get and set collision box
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        public Rectangle CollisionBox
        {
            get { return this.collisionBox; }
            set { this.collisionBox = value; }
        }

        public List<Rockets> Rockets
        {
            get { return this.rockets; }
            set { this.rockets = value; }
        }

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

            sounds.Add(content.Load<SoundEffect>("jump"));
            sounds.Add(content.Load<SoundEffect>("sweep"));
            sounds.Add(content.Load<SoundEffect>("stone"));
            sounds.Add(content.Load<SoundEffect>("lasershot"));
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

            if (slideCDTimer > 0)
                slideCDTimer -= deltaTime;
            else
                slideCD = false;


            if (sliding)
            {
                if (currentDirection == PlayerDirection.Left)
                    spriteDirection += new Vector2(-1f, 0f);
                else
                    spriteDirection += new Vector2(1f, 0f);
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
            JumpBoostCheckTimer();
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
                if (Keyboard.IsKeyDown(Keys.Left)) /// LEFT ARROW KEY
                    handleArrowKey("Left", Keyboard);

                else if (Keyboard.IsKeyDown(Keys.Right)) /// RIGHT KEY 
                    handleArrowKey("Right", Keyboard);

                else if (Keyboard.IsKeyDown(Keys.Space)) /// SPACE KEY
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
                    sounds[0].Play();

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

            }
            UpdateRocket();
        }

        private void handleArrowKey(string direction, KeyboardState Keyboard) // direction == Left
        {
            if (jumped)
            {
                if (Keyboard.IsKeyDown(Keys.X))
                {
                    attacking = true;
                    PlayAnimation("Melee" + direction);
                }
                else
                    PlayAnimation("Jump" + direction);
            }

            if (direction == "Left")
                spriteDirection += new Vector2(-1f, 0f);
            else
                spriteDirection += new Vector2(1f, 0f);

            if (Keyboard.IsKeyDown(Keys.Down) && !jumped && !slideCD) /// SLIDE 
            {
                sliding = true;
                PlayAnimation("Slide" + direction);
                sounds[1].Play();
            }
            else if (Keyboard.IsKeyDown(Keys.Space) && !jumped) /// SHOOT
            {
                attacking = true;
                PlayAnimation("Attack" + direction);
                Shoot();

            }
            else if (Keyboard.IsKeyDown(Keys.Up) && !jumped) /// JUMP
            {
                jumped = true;
                VelocityY = jumpHeight;
                spriteDirection += new Vector2(0f, 1f);
                sounds[0].Play();

                if (Keyboard.IsKeyDown(Keys.X))
                {
                    attacking = true;
                    PlayAnimation("Melee" + direction);
                }
                else
                    PlayAnimation("Jump" + direction);
            }
            else if (!jumped) /// RUN
                PlayAnimation("Run" + direction);

            if (direction == "Left")
                currentDirection = PlayerDirection.Left;
            else
                currentDirection = PlayerDirection.Right;

        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Check if specific animation has ended
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack") || animation.Contains("Shoot"))
                attacking = false;

            if (animation.Contains("Slide"))
            {
                slideCDTimer = 0.50f;
                slideCD = true;
                sliding = false;
            }
        }


        public void CollisionWithMap(Rectangle tileRectangle, int mapWidth, int mapHeight)
        {
            if (!collisionBox.TouchTopOf(tileRectangle) && !jumped)
                grounded = false;
            if (collisionBox.TouchTopOf(tileRectangle))
            {
                if (sliding)
                    playerPosition.Y = tileRectangle.Y - collisionBox.Height - 25;
                else
                    playerPosition.Y = tileRectangle.Y - collisionBox.Height - 9;

                VelocityY = 0.0f;
                jumped = false;
                grounded = true;
            }
            if (collisionBox.TouchLeftOf(tileRectangle) && currentDirection == PlayerDirection.Right)
            {
                sliding = false;
                playerPosition.X -= stepAmount.X;
            }
            if (collisionBox.TouchRightOf(tileRectangle) && currentDirection == PlayerDirection.Left)
            {
                sliding = false;
                playerPosition.X -= stepAmount.X;
            }
            if (collisionBox.TouchBottomOf(tileRectangle))
                VelocityY = -1.0f;
        }

        public void CollisionWithEntity(IEntity entity)
        {
            //TODO: player collision with enemy
            if (this.collisionBox.Intersects(entity.CollisionBox))
            {
                this.playerHP--;
            }


        }

        /// <summary>
        /// If player colide with collectable item this function check the bonus type
        /// (health or jump foe example) and up health or set on the JumpBoosterTimer
        /// The player`s health can`t be more than maximum of 350 health points.
        /// </summary>
        /// <param name="collectable"></param>
        public void CollisionWithCollectable(ICollectable collectable)
        {
            if (this.collisionBox.Intersects(collectable.CollisionBox))
            {
                if (collectable.RestoreHealthPoints > 0)
                {
                    this.playerHP += collectable.RestoreHealthPoints;
                    if (this.playerHP > 350)
                    {
                        this.playerHP = 350;
                    }
                }



                if (collectable.JumpBoost > 0)
                {
                    JumpBoostTimer = 2000;
                }


            }
        }

        /// <summary>
        /// If the JumpBoosterTimer has a time this function set player to jump high.
        /// </summary>
        public void JumpBoostCheckTimer()
        {
            if (JumpBoostTimer > 1)
            {
                JumpBoostTimer--;
                this.jumpHeight = 450;
            }
            else
            {
                this.jumpHeight = 400;
            }
        }

        public void CollisionWithRocket(Rockets rocket, Player player)
        {
            //TODO: player collision with rockets
        }



        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Set all collision bounds
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        private void setCollisionBounds()
        {
            switch (currentAnimation)
            {
                case "IdleLeft": setCollisionRectangle(24, 10, 47, 83); break;
                case "IdleRight": setCollisionRectangle(30, 10, 47, 83); break;
                case "RunRight": setCollisionRectangle(10, 10, 57, 83); break;
                case "RunLeft": setCollisionRectangle(33, 10, 57, 83); break;
                case "AttackRight": setCollisionRectangle(14, 10, 70, 83); break;
                case "AttackLeft": setCollisionRectangle(16, 10, 70, 83); break;
                case "JumpRight": setCollisionRectangle(14, 10, 66, 83); break;
                case "JumpLeft": setCollisionRectangle(20, 10, 66, 83); break;
                case "MeleeRight": setCollisionRectangle(13, 10, 75, 88); break;
                case "MeleeLeft": setCollisionRectangle(12, 10, 75, 88); break;
                case "ShootRight": setCollisionRectangle(21, 10, 57, 83); break;
                case "ShootLeft": setCollisionRectangle(22, 10, 56, 83); break;
                case "IdleMeleeRight": setCollisionRectangle(15, 10, 70, 83); break;
                case "IdleMeleeLeft": setCollisionRectangle(18, 10, 70, 83); break;
                case "SlideRight": setCollisionRectangle(8, 30, 68, 65); break;
                case "SlideLeft": setCollisionRectangle(23, 30, 68, 65); break;
                default: setCollisionRectangle(0, 0, 100, 100); break;
            }
            collisionBox = new Rectangle(((int)playerPosition.X + xOffset), ((int)playerPosition.Y + yOffset), width, height);
        }

        private void setCollisionRectangle(int x, int y, int w, int h)
        {
            xOffset = x;
            yOffset = y;
            width = w;
            height = h;
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
                    sounds[3].Play();
                }
                else
                {
                    newRocket = new Rockets(shootTextureRight);
                    newRocket.shootPosition = new Vector2(playerPosition.X + 80, playerPosition.Y + 35);
                    newRocket.direction = "right";
                    sounds[3].Play();
                }

                newRocket.isVisible = true; // set current rocket's visibility to true

                // Add rocket to list if they are < 20

                if (rockets.Count < 20)
                    rockets.Add(newRocket);
            }

            //Reset rocket delay _jumpBoostJumpBoostTimer

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

                if (rocket.shootPosition.X < Camera.centre.X - 640 || rocket.shootPosition.X > Camera.centre.X + 640)
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