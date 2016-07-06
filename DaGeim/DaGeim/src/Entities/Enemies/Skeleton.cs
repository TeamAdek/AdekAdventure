using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

internal sealed class Skeleton : NPC
{

    public Skeleton(Vector2 position, int range)
        : base(position, range)
    {
        Health = 100;
        FramesPerSecond = 12;
        ammoType = typeof(Icycle);
       

        LoadAnimations();
        PlayAnimation("IdleLeft");
        entityOrientation = Orientations.Left;

    }

    public override void LoadContent(ContentManager content)
    {
        entityTexture = content.Load<Texture2D>("SpriteSheetEnemy2");
        ammoLeft = content.Load<Texture2D>("icycleLeft");
        ammoRight = content.Load<Texture2D>("icycleRight");
    }

    public override void Shoot(Player player)
    {
        if (PlayerIsInRange(player) && !isDying)
        {
            //PlayAnimation("ShootLeft");
            if (shootCD > 0.0f)
                shootCD--;
            if (shootCD == 0.0f)
            {
                if (player.Position.X < entityPosition.X)
                    entityOrientation = Orientations.Left;
                else if (player.Position.X >= entityPosition.X)
                    entityOrientation = Orientations.Right;

                Icycle icycle;
                if (entityOrientation == Orientations.Left)
                {
                    icycle = new Icycle(new Vector2(entityPosition.X - 15, entityPosition.Y + 35), "left", ammoLeft,
                        ammoRight);
                    PlayAnimation("ShootLeft");
                    IsAttacking = true;
                }
                else
                {
                    icycle = new Icycle(new Vector2(entityPosition.X + 80, entityPosition.Y + 35), "right", ammoLeft, ammoRight);
                    PlayAnimation("ShootRight");
                    IsAttacking = true;
                }
                ammo.Add(icycle);
                shootCD = 40.0f;
            }
        }
    }

public override void Update(GameTime gameTime)
    {
        UpdateAmmo(gameTime);
        if (!isDying)
        {


            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            entityPosition += entityVelocity;
            UpdateCollisionBounds();

            if (entityVelocity.Y < 10) /// used as gravity
                entityVelocity.Y += 0.4f;

            if (!IsAttacking)
                Patrol();
        }
        base.Update(gameTime);
    }

    public override void UpdateCollisionBounds()
    {
        Rectangle output = new Rectangle();
        switch (currAnimationSet)
        {
            case "IdleLeft": output = SetCollisionRectangle(6, 0, 64, 90); break;
            case "IdleRight": output = SetCollisionRectangle(0, 0, 64, 90); break;
            case "WalkLeft": output = SetCollisionRectangle(3, 0, 59, 90); break;
            case "WalkRight": output = SetCollisionRectangle(8, 0, 59, 90); break;
            default: output = SetCollisionRectangle(0, 0, 70, 90); break;
        }
        CollisionBox = output;
    }

    protected override void LoadAnimations()
    {
        textureWidth = 110;
        textureHeight = 95;
        AddAnimation("IdleLeft", 10, 0);
        AddAnimation("IdleRight", 10, 1);
        AddAnimation("WalkLeft", 10, 2);
        AddAnimation("WalkRight", 10, 3);
        AddAnimation("ShootLeft", 10, 4);
        AddAnimation("ShootRight", 10, 5);
        AddAnimation("DieLeft", 10, 6);
        AddAnimation("DieRight", 10, 7);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var icycle in ammo)
        {
            icycle.Draw(spriteBatch);
        }
        base.Draw(spriteBatch, entityPosition, entityTexture);
    }

    protected override void AnimationDone(string animation)
    {
        if (animation.Contains("Shoot"))
            IsAttacking = false;
        if (animation.Contains("Die"))
            Dead = true;
    }
}
