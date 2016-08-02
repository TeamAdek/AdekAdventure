namespace DaGeim.Entities.Player
{
    using System.Collections.Generic;
    using Ammunition;
    using Helper_Classes;
    using Interfaces;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public sealed class Player : Entity, ICollecting, IJumpboostable
    {
        private float slideCDTimer;
        private float rocketCDTimer;
        private readonly float gravity;
        private float jumpHeight;
        private Vector2 stepAmount;
        private int score;
        private bool attacking, sliding, slideCD, jumped, grounded;

        private Texture2D rocketLeft;
        private Texture2D rocketRight;
        private List<Rocket> rockets;

        private int jumpBoostJumpBoostTimer;
        private float rocketFrequencyShootingTimer = 50.0f;

        public Player(Vector2 position)
            : base(position)
        {
            this.Health = 300;
            this.FramesPerSecond = 10;

            this.entityVelocity.X = 250.0f;
            this.entityVelocity.Y = 0.0f;

            this.gravity = 15.0f;
            this.jumpHeight = 400.0f;
            this.score = 0;
            this.rockets = new List<Rocket>();

            this.LoadAnimations();
            this.PlayAnimation("IdleRight");
            this.entityOrientation = Orientations.Right;
        }

        public int JumpBoostTimer
        {
            get { return jumpBoostJumpBoostTimer; }
            set { jumpBoostJumpBoostTimer = value; }
        }

        public int Score
        {
            get { return this.score; }
            set
            {
                this.score += value;
                if (this.score < 0)
                    this.score = 0;
            }
        }

        public List<Rocket> Rockets { get { return this.rockets; } }
        public override void Update(GameTime gameTime)
        {
            if (this.jumped)
                this.entityDirection = new Vector2(0f, 1f);
            else
                this.entityDirection = Vector2.Zero;

            this.HandleInput(Keyboard.GetState());
            this.UpdateRockets(gameTime);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.slideCDTimer > 0)
                this.slideCDTimer -= deltaTime;
            else
                this.slideCD = false;

            if (this.sliding)
            {
                if (this.entityOrientation == Orientations.Left)
                    this.entityDirection += new Vector2(-1f, 0f);
                else
                    this.entityDirection += new Vector2(1f, 0f);
            }

            if (!this.grounded && !this.jumped)
            {
                this.entityDirection += new Vector2(0, 3.0f);
            }

            this.entityVelocity.Y += this.gravity * (-1.0f);
            this.entityDirection *= new Vector2(this.entityVelocity.X, -this.entityVelocity.Y);
            this.entityPosition += (this.entityDirection * deltaTime);

            this.stepAmount = this.entityDirection * deltaTime;

            this.UpdateCollisionBounds();
            //  TODO: Implement this whole JumpBoostThingy --> JumpBoostCheckTimer();
            this.JumpBoostCheckTimer();


            base.Update(gameTime);
        }

        private void HandleInput(KeyboardState keyboard)
        {
            if (!this.sliding)
            {
                if (keyboard.IsKeyDown(Keys.Left)) // LEFT ARROW KEY
                    this.HandleArrowKey("Left", keyboard);

                else if (keyboard.IsKeyDown(Keys.Right)) // RIGHT KEY 
                    this.HandleArrowKey("Right", keyboard);

                else if (keyboard.IsKeyDown(Keys.Space)) // SPACE KEY
                {
                    this.attacking = true;
                    if (this.entityOrientation == Orientations.Left)
                    {
                        this.entityOrientation = Orientations.Left;
                        this.PlayAnimation("ShootLeft");
                        this.Shoot();

                    }
                    else if (this.entityOrientation == Orientations.Right)
                    {
                        this.entityOrientation = Orientations.Right;
                        this.PlayAnimation("ShootRight");
                        this.Shoot();
                    }
                }
                // UP ARROW
                else if (keyboard.IsKeyDown(Keys.Up) && !this.jumped)
                {
                    this.jumped = true;
                    this.entityDirection += new Vector2(0f, 1f);
                    this.entityVelocity.Y = this.jumpHeight;
                    // sounds[0].Play();

                    if (this.entityOrientation == Orientations.Left)
                        this.PlayAnimation("JumpLeft");
                    else
                        this.PlayAnimation("JumpRight");
                }
                else if (keyboard.IsKeyDown(Keys.X))
                {
                    this.attacking = true;
                    if (this.entityOrientation == Orientations.Left)
                        this.PlayAnimation("IdleMeleeLeft");
                    else
                        this.PlayAnimation("IdleMeleeRight");
                }
                // NO KEY PRESSED
                else
                {
                    if (!this.jumped)
                    {
                        if (this.entityOrientation == Orientations.Left)
                            this.PlayAnimation("IdleLeft");
                        if (this.entityOrientation == Orientations.Right)
                            this.PlayAnimation("IdleRight");
                    }
                }

            }
            // UpdateRocket();
        }

        private void HandleArrowKey(string direction, KeyboardState keyboard)
        {
            if (this.jumped)
            {
                if (keyboard.IsKeyDown(Keys.X))
                {
                    this.attacking = true;
                    this.PlayAnimation("Melee" + direction);
                }
                else
                    this.PlayAnimation("Jump" + direction);
            }

            if (direction == "Left")
                this.entityDirection += new Vector2(-1f, 0f);
            else
                this.entityDirection += new Vector2(1f, 0f);

            if (keyboard.IsKeyDown(Keys.Down) && !jumped && !slideCD) // SLIDE 
            {
                this.sliding = true;
                this.PlayAnimation("Slide" + direction);
                //sounds[1].Play();
            }
            else if (keyboard.IsKeyDown(Keys.Space) && !jumped) // SHOOT
            {
                this.attacking = true;
                this.PlayAnimation("Attack" + direction);
                this.Shoot();

            }
            else if (keyboard.IsKeyDown(Keys.Up) && !jumped) // JUMP
            {
                this.jumped = true;
                this.entityVelocity.Y = this.jumpHeight;
                this.entityDirection += new Vector2(0f, 1f);
                //sounds[0].Play();

                if (keyboard.IsKeyDown(Keys.X))
                {
                    this.attacking = true;
                    this.PlayAnimation("Melee" + direction);
                }
                else
                    this.PlayAnimation("Jump" + direction);
            }
            else if (!this.jumped) // RUN
                this.PlayAnimation("Run" + direction);

            if (direction == "Left")
                this.entityOrientation = Orientations.Left;
            else
                this.entityOrientation = Orientations.Right;
        }

        private void Shoot()
        {
            if (this.rocketCDTimer > 0.0f)
                this.rocketCDTimer--;

            if (this.rocketCDTimer.Equals(0.0f))
            {
                Rocket rocket;
                if (this.entityOrientation == Orientations.Left)
                    rocket = new Rocket(new Vector2(this.entityPosition.X + -15, this.entityPosition.Y + 35), "left", this.rocketLeft, this.rocketRight);
                else
                    rocket = new Rocket(new Vector2(this.entityPosition.X + 80, this.entityPosition.Y + 35), "right", this.rocketLeft, this.rocketRight);

                this.rockets.Add(rocket);
                //TODO: Play sound here (on this line)
                this.rocketCDTimer = rocketFrequencyShootingTimer;
            }
        }

        private void UpdateRockets(GameTime gameTime)
        {
            foreach (var rocket in this.rockets)
                rocket.Update(gameTime);

            for (int i = 0; i < this.rockets.Count; i++)
                if (!this.rockets[i].IsVisible)
                {
                    this.rockets.RemoveAt(i);
                    i--;
                }
        }

        public void PushedByBoss()
        {
            this.sliding = true;
            this.PlayAnimation("SlideLeft");
            this.entityOrientation = Orientations.Left;
            //TO DO: Add sound
        }

        public override void CollisionWithMap(Rectangle tileRectangle, int mapWidth, int mapHeight)
        {
            if (!CollisionBox.TouchTopOf(tileRectangle) && !this.jumped)
                this.grounded = false;
            if (CollisionBox.TouchTopOf(tileRectangle))
            {
                if (this.sliding)
                    this.entityPosition.Y = tileRectangle.Y - CollisionBox.Height - 25;
                else
                    this.entityPosition.Y = tileRectangle.Y - CollisionBox.Height - 9;

                this.entityVelocity.Y = 0.0f;
                this.jumped = false;
                this.grounded = true;
            }
            if (CollisionBox.TouchLeftOf(tileRectangle) && entityOrientation == Orientations.Right)
            {
                this.sliding = false;
                this.entityPosition.X -= stepAmount.X;
            }
            if (CollisionBox.TouchRightOf(tileRectangle) && entityOrientation == Orientations.Left)
            {
                this.sliding = false;
                this.entityPosition.X -= stepAmount.X;
            }
            if (CollisionBox.TouchBottomOf(tileRectangle))
                this.entityVelocity.Y = -1.0f;
        }

        public void CollisionWithCollectable(ICollectable collectable)
        {
            if (this.CollisionBox.Intersects(collectable.CollisionBox))
            {
                if (collectable.RestoreHealthPoints > 0)
                {
                    this.Health += collectable.RestoreHealthPoints;
                    if (this.Health > 320)
                    {
                        this.Health = 320;
                    }
                }

                if (collectable.BonusScorePoints > 0)
                {
                    this.score += collectable.BonusScorePoints;
                }

                if (collectable.JumpBoost > 0)
                {
                    this.JumpBoostTimer = 2000;
                }

                if (collectable.BonusRockerShootingBooster > 0)
                {
                    this.rocketFrequencyShootingTimer = collectable.BonusRockerShootingBooster;
                }


            }
        }

        public override void LoadContent(ContentManager content)
        {
            this.entityTexture = content.Load<Texture2D>("PlayerAnimation");
            this.rocketLeft = content.Load<Texture2D>("rocketLeft");
            this.rocketRight = content.Load<Texture2D>("rocketRight");

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //TODO: Handle rocket updating.
            base.Draw(spriteBatch, entityPosition, entityTexture);
        }

        protected override void AnimationDone(string animation)
        {
            if (animation.Contains("Attack") || animation.Contains("Shoot"))
                this.attacking = false;

            if (animation.Contains("Slide"))
            {
                this.slideCDTimer = 0.50f;
                this.slideCD = true;
                this.sliding = false;
            }
        }

        public override void UpdateCollisionBounds()
        {
            Rectangle output = new Rectangle();
            switch (this.currAnimationSet)
            {
                case "IdleLeft": output = SetCollisionRectangle(24, 10, 47, 83); break;
                case "IdleRight": output = SetCollisionRectangle(30, 10, 47, 83); break;
                case "RunRight": output = SetCollisionRectangle(10, 10, 57, 83); break;
                case "RunLeft": output = SetCollisionRectangle(33, 10, 57, 83); break;
                case "AttackRight": output = SetCollisionRectangle(14, 10, 70, 83); break;
                case "AttackLeft": output = SetCollisionRectangle(16, 10, 70, 83); break;
                case "JumpRight": output = SetCollisionRectangle(14, 10, 66, 83); break;
                case "JumpLeft": output = SetCollisionRectangle(20, 10, 66, 83); break;
                case "MeleeRight": output = SetCollisionRectangle(13, 10, 75, 88); break;
                case "MeleeLeft": output = SetCollisionRectangle(12, 10, 75, 88); break;
                case "ShootRight": output = SetCollisionRectangle(21, 10, 57, 83); break;
                case "ShootLeft": output = SetCollisionRectangle(22, 10, 56, 83); break;
                case "IdleMeleeRight": output = SetCollisionRectangle(15, 10, 70, 83); break;
                case "IdleMeleeLeft": output = SetCollisionRectangle(18, 10, 70, 83); break;
                case "SlideRight": output = SetCollisionRectangle(8, 30, 68, 65); break;
                case "SlideLeft": output = SetCollisionRectangle(23, 30, 68, 65); break;
                default: output = SetCollisionRectangle(0, 0, 100, 100); break;
            }
            CollisionBox = output;
        }

        protected override void LoadAnimations()
        {
            this.textureWidth = 100;
            this.textureHeight = 100;
            AddAnimation("IdleRight", 10, 0);
            AddAnimation("IdleLeft", 10, 1);
            AddAnimation("RunRight", 8, 2);
            AddAnimation("RunLeft", 8, 3);
            AddAnimation("AttackRight", 9, 4);
            AddAnimation("AttackLeft", 9, 5);
            AddAnimation("JumpRight", 10, 6);
            AddAnimation("JumpLeft", 10, 7);
            AddAnimation("MeleeRight", 8, 8);
            AddAnimation("MeleeLeft", 8, 9);
            AddAnimation("ShootRight", 4, 10);
            AddAnimation("ShootLeft", 4, 11);
            AddAnimation("IdleMeleeRight", 8, 12);
            AddAnimation("IdleMeleeLeft", 8, 13);
            AddAnimation("SlideRight", 10, 14);
            AddAnimation("SlideLeft", 10, 15);
        }


        /// <summary>
        /// If the JumpBoosterTimer has a time this function set player to jump high.
        /// </summary>
        public void JumpBoostCheckTimer()
        {
            if (this.JumpBoostTimer > 1)
            {
                this.JumpBoostTimer--;
                this.jumpHeight = 450;
            }
            else
            {
                this.jumpHeight = 400;
            }
        }
    }

}
