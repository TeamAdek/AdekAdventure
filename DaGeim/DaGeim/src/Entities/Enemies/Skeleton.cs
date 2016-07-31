namespace DaGeim.Entities.Enemies
{
    using Ammunition;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using Player = Player.Player;

    public sealed class Skeleton : NPC
    {

        public Skeleton(Vector2 position, int range)
            : base(position, range)
        {
            this.Health = 100;
            this.FramesPerSecond = 12;
            this.ammoType = typeof(Icycle);
       

            this.LoadAnimations();
            this.PlayAnimation("IdleLeft");
            this.entityOrientation = Orientations.Left;

        }

        public override void LoadContent(ContentManager content)
        {
            this.entityTexture = content.Load<Texture2D>("SpriteSheetEnemy2");
            this.ammoLeft = content.Load<Texture2D>("icycleLeft");
            this.ammoRight = content.Load<Texture2D>("icycleRight");
        }

        public override void Shoot(Player player)
        {
            if (this.PlayerIsInRange(player) && !this.isDying)
            {
                //PlayAnimation("ShootLeft");
                if (this.shootCD > 0.0f)
                    this.shootCD--;
                if (this.shootCD == 0.0f)
                {
                    if (player.Position.X < this.entityPosition.X)
                        this.entityOrientation = Orientations.Left;
                    else if (player.Position.X >= this.entityPosition.X)
                        this.entityOrientation = Orientations.Right;

                    Icycle icycle;
                    if (this.entityOrientation == Orientations.Left)
                    {
                        icycle = new Icycle(new Vector2(this.entityPosition.X - 15, this.entityPosition.Y + 35), "left", this.ammoLeft,
                            this.ammoRight);
                        this.PlayAnimation("ShootLeft");
                        this.IsAttacking = true;
                    }
                    else
                    {
                        icycle = new Icycle(new Vector2(this.entityPosition.X + 80, this.entityPosition.Y + 35), "right", this.ammoLeft, this.ammoRight);
                        this.PlayAnimation("ShootRight");
                        this.IsAttacking = true;
                    }
                    this.ammo.Add(icycle);
                    this.shootCD = 40.0f;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateAmmo(gameTime);
            if (!this.isDying)
            {


                float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
                this.entityPosition += this.entityVelocity;
                this.UpdateCollisionBounds();

                if (this.entityVelocity.Y < 10) /// used as gravity
                    this.entityVelocity.Y += 0.4f;

                if (!this.IsAttacking)
                    this.Patrol();
            }
            base.Update(gameTime);
        }

        public override void UpdateCollisionBounds()
        {
            Rectangle output = new Rectangle();
            switch (this.currAnimationSet)
            {
                case "IdleLeft": output = this.SetCollisionRectangle(6, 0, 64, 90); break;
                case "IdleRight": output = this.SetCollisionRectangle(0, 0, 64, 90); break;
                case "WalkLeft": output = this.SetCollisionRectangle(3, 0, 59, 90); break;
                case "WalkRight": output = this.SetCollisionRectangle(8, 0, 59, 90); break;
                default: output = this.SetCollisionRectangle(0, 0, 70, 90); break;
            }
            this.CollisionBox = output;
        }

        protected override void LoadAnimations()
        {
            this.textureWidth = 110;
            this.textureHeight = 95;
            this.AddAnimation("IdleLeft", 10, 0);
            this.AddAnimation("IdleRight", 10, 1);
            this.AddAnimation("WalkLeft", 10, 2);
            this.AddAnimation("WalkRight", 10, 3);
            this.AddAnimation("ShootLeft", 10, 4);
            this.AddAnimation("ShootRight", 10, 5);
            this.AddAnimation("DieLeft", 10, 6);
            this.AddAnimation("DieRight", 10, 7);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var icycle in this.ammo)
            {
                icycle.Draw(spriteBatch);
            }
            base.Draw(spriteBatch, this.entityPosition, this.entityTexture);
        }

        protected override void AnimationDone(string animation)
        {
            if (animation.Contains("Shoot"))
                this.IsAttacking = false;
            if (animation.Contains("Die"))
                this.Dead = true;
        }
    }
}
