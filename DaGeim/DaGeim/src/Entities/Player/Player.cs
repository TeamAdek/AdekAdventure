using System;
using System.Collections.Generic;
using DaGeim;
using DaGeim.Interfaces;
using DaGeim.src.Entities.New_Code;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

internal sealed class Player : Entity, ICollecting, IJumpboostable
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

    private int jumpBoostJumpBoostTimer = 0;
    private float rocketFrequencyShootingTimer = 50.0f;

    public Player(Vector2 position)
        : base(position)
    {
        Health = 300;
        FramesPerSecond = 10;

        entityVelocity.X = 250.0f;
        entityVelocity.Y = 0.0f;

        gravity = 15.0f;
        jumpHeight = 400.0f;
        score = 0;
        rockets = new List<Rocket>();

        LoadAnimations();
        PlayAnimation("IdleRight");
        entityOrientation = Orientations.Right;
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
            score += value;
            if (score < 0)
                score = 0;
        }
    }

    public List<Rocket> Rockets { get { return this.rockets; } }
    public override void Update(GameTime gameTime)
    {
        if (jumped)
            entityDirection = new Vector2(0f, 1f);
        else
            entityDirection = Vector2.Zero;

        HandleInput(Keyboard.GetState());
        UpdateRockets(gameTime);
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (slideCDTimer > 0)
            slideCDTimer -= deltaTime;
        else
            slideCD = false;

        if (sliding)
        {
            if (entityOrientation == Orientations.Left)
                entityDirection += new Vector2(-1f, 0f);
            else
                entityDirection += new Vector2(1f, 0f);
        }

        if (!grounded && !jumped)
        {
            entityDirection += new Vector2(0, 3.0f);
        }

        entityVelocity.Y += gravity * (-1.0f);
        entityDirection *= new Vector2(entityVelocity.X, -entityVelocity.Y);
        entityPosition += (entityDirection * deltaTime);
        
        stepAmount = entityDirection * deltaTime;

        UpdateCollisionBounds();
        //  TODO: Implement this whole JumpBoostThingy --> JumpBoostCheckTimer();
        JumpBoostCheckTimer();


        base.Update(gameTime);
    }

    private void HandleInput(KeyboardState keyboard)
    {
        if (!sliding)
        {
            if (keyboard.IsKeyDown(Keys.Left)) /// LEFT ARROW KEY
                HandleArrowKey("Left", keyboard);

            else if (keyboard.IsKeyDown(Keys.Right)) /// RIGHT KEY 
                HandleArrowKey("Right", keyboard);

            else if (keyboard.IsKeyDown(Keys.Space)) /// SPACE KEY
            {
                attacking = true;
                if (entityOrientation == Orientations.Left)
                {
                    entityOrientation = Orientations.Left;
                    PlayAnimation("ShootLeft");
                    Shoot();

                }
                else if (entityOrientation == Orientations.Right)
                {
                    entityOrientation = Orientations.Right;
                    PlayAnimation("ShootRight");
                    Shoot();
                }
            }
            /// UP ARROW
            else if (keyboard.IsKeyDown(Keys.Up) && !jumped)
            {
                jumped = true;
                entityDirection += new Vector2(0f, 1f);
                entityVelocity.Y = jumpHeight;
                // sounds[0].Play();

                if (entityOrientation == Orientations.Left)
                    PlayAnimation("JumpLeft");
                else
                    PlayAnimation("JumpRight");
            }
            else if (keyboard.IsKeyDown(Keys.X))
            {
                attacking = true;
                if (entityOrientation == Orientations.Left)
                    PlayAnimation("IdleMeleeLeft");
                else
                    PlayAnimation("IdleMeleeRight");
            }
            /// NO KEY PRESSED
            else
            {
                if (!jumped)
                {
                    if (entityOrientation == Orientations.Left)
                        PlayAnimation("IdleLeft");
                    if (entityOrientation == Orientations.Right)
                        PlayAnimation("IdleRight");
                }
            }

        }
        // UpdateRocket();
    }

    private void HandleArrowKey(string direction, KeyboardState keyboard)
    {
        if (jumped)
        {
            if (keyboard.IsKeyDown(Keys.X))
            {
                attacking = true;
                PlayAnimation("Melee" + direction);
            }
            else
                PlayAnimation("Jump" + direction);
        }

        if (direction == "Left")
            entityDirection += new Vector2(-1f, 0f);
        else
            entityDirection += new Vector2(1f, 0f);

        if (keyboard.IsKeyDown(Keys.Down) && !jumped && !slideCD) /// SLIDE 
        {
            sliding = true;
            PlayAnimation("Slide" + direction);
            //sounds[1].Play();
        }
        else if (keyboard.IsKeyDown(Keys.Space) && !jumped) /// SHOOT
        {
            attacking = true;
            PlayAnimation("Attack" + direction);
            Shoot();

        }
        else if (keyboard.IsKeyDown(Keys.Up) && !jumped) /// JUMP
        {
            jumped = true;
            entityVelocity.Y = jumpHeight;
            entityDirection += new Vector2(0f, 1f);
            //sounds[0].Play();

            if (keyboard.IsKeyDown(Keys.X))
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
            entityOrientation = Orientations.Left;
        else
            entityOrientation = Orientations.Right;
    }

    private void Shoot()
    {
        if (rocketCDTimer > 0.0f)
            rocketCDTimer--;

        if (rocketCDTimer == 0.0f)
        {
            Rocket rocket;
            if (entityOrientation == Orientations.Left)
                rocket = new Rocket(new Vector2(entityPosition.X + -15, entityPosition.Y + 35), "left", rocketLeft, rocketRight);
            else
                rocket = new Rocket(new Vector2(entityPosition.X + 80, entityPosition.Y + 35), "right", rocketLeft, rocketRight);

            rockets.Add(rocket);
            ///TODO: Play sound here (on this line)
            rocketCDTimer = rocketFrequencyShootingTimer;
        }
    }

    private void UpdateRockets(GameTime gameTime)
    {
        foreach (var rocket in rockets)
            rocket.Update(gameTime);

        for (int i = 0; i < rockets.Count; i++)
            if (!rockets[i].IsVisible)
            {
                rockets.RemoveAt(i);
                i--;
            }
    }

    public void PushedByBoss()
    {
        sliding = true;
        PlayAnimation("SlideLeft");
        entityOrientation = Orientations.Left;
        //TO DO: Add sound
    }

    public override void CollisionWithMap(Rectangle tileRectangle, int mapWidth, int mapHeight)
    {
        if (!CollisionBox.TouchTopOf(tileRectangle) && !jumped)
            grounded = false;
        if (CollisionBox.TouchTopOf(tileRectangle))
        {
            if (sliding)
                entityPosition.Y = tileRectangle.Y - CollisionBox.Height - 25;
            else
                entityPosition.Y = tileRectangle.Y - CollisionBox.Height - 9;

            entityVelocity.Y = 0.0f;
            jumped = false;
            grounded = true;
        }
        if (CollisionBox.TouchLeftOf(tileRectangle) && entityOrientation == Orientations.Right)
        {
            sliding = false;
            entityPosition.X -= stepAmount.X;
        }
        if (CollisionBox.TouchRightOf(tileRectangle) && entityOrientation == Orientations.Left)
        {
            sliding = false;
            entityPosition.X -= stepAmount.X;
        }
        if (CollisionBox.TouchBottomOf(tileRectangle))
            entityVelocity.Y = -1.0f;
    }

    public void CollisionWithCollectable(ICollectable collectable)
    {
        if (this.CollisionBox.Intersects(collectable.CollisionBox))
        {
            if (collectable.RestoreHealthPoints > 0)
            {
                Health += collectable.RestoreHealthPoints;
                if (Health > 320)
                {
                    Health = 320;
                }
            }

            if (collectable.BonusScorePoints > 0)
            {
                score += collectable.BonusScorePoints;
            }

            if (collectable.JumpBoost > 0)
            {
                JumpBoostTimer = 2000;
            }

            if (collectable.BonusRockerShootingBooster > 0)
            {
                rocketFrequencyShootingTimer = collectable.BonusRockerShootingBooster;
            }


        }
    }

    public override void LoadContent(ContentManager content)
    {
        entityTexture = content.Load<Texture2D>("PlayerAnimation");
        rocketLeft = content.Load<Texture2D>("rocketLeft");
        rocketRight = content.Load<Texture2D>("rocketRight");

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        ///TODO: Handle rocket updating.
        base.Draw(spriteBatch, entityPosition, entityTexture);
    }

    protected override void AnimationDone(string animation)
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

    public override void UpdateCollisionBounds()
    {
        Rectangle output = new Rectangle();
        switch (currAnimationSet)
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
        textureWidth = 100;
        textureHeight = 100;
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
}
