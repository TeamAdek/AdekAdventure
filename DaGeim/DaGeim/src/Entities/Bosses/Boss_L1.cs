namespace DaGeim.Entities.Bosses
{
    using Ammunition;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Player = Player.Player;

    public sealed class Boss_L1 : NPC
    {

        public bool isPushing = false;

        public Boss_L1(Vector2 position, int range): base(position, range)
        {
            this.Health = 180;
            this.FramesPerSecond = 15;
            this.ammoType = typeof(Laser);
            this.LoadAnimations();
            this.PlayAnimation("WalkRight");
            this.entityOrientation = Orientations.Right;
        }

        public override void LoadContent(ContentManager content)
        {
            this.entityTexture = content.Load<Texture2D>("SpriteSheetBoss");
            this.ammoLeft = content.Load<Texture2D>("blastEdit");
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateAmmo(gameTime);

            if (!this.isDying)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                //entityPosition += entityVelocity;
                this.UpdateCollisionBounds();

                if (this.entityVelocity.Y < 10) /// used as gravity
                    this.entityVelocity.Y += 0.4f;

                if (!this.IsAttacking)
                    this.Patrol();
            }

            base.Update(gameTime);
        }

        public override void Shoot(Player player)
        {
            int distanceToPlayer = (int)this.entityPosition.X - (int)player.Position.X;

            if (distanceToPlayer < 100)
            {
                this.PlayAnimation("Push");
                if (!this.isPushing)
                    this.entityPosition.Y -= 47;
                this.notPatrolling = true;
                this.isPushing = true;
            }

            base.Shoot(player);
        }

        public override void UpdateCollisionBounds()
        {
            Rectangle output = new Rectangle();
            switch (this.currAnimationSet)
            {
                case "WalkLeft": output = this.SetCollisionRectangle(30, 0, 45, 123); break;
                case "WalkRight": output = this.SetCollisionRectangle(30, 0, 45, 123); break;
                case "ShootLeft": output = this.SetCollisionRectangle(30, 0, 45, 123); break;
                case "Push": output = this.SetCollisionRectangle(30, 0, 45, 123); break;
                default: output = this.SetCollisionRectangle(30, 0, 45, 123); break;
            }
            this.CollisionBox = output;
        }

        protected override void LoadAnimations()
        {
            this.textureWidth = 128;
            this.textureHeight = 128;
            this.AddAnimation("WalkLeft", 12, 0);
            this.AddAnimation("WalkRight", 12, 1);
            this.AddAnimation("ShootLeft", 12, 2);
            this.AddAnimation("DieLeft", 12, 3);
            this.AddAnimation("DieRight", 12, 3);
            this.offSetYPx += 47;
            this.AddAnimation("Push", 12, 4);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var laser in this.ammo)
                laser.Draw(spriteBatch);

            base.Draw(spriteBatch, this.entityPosition, this.entityTexture);
        }

        protected override void AnimationDone(string animation)
        {
            if (animation.Contains("Shoot"))
                this.IsAttacking = false;
            if (animation.Contains("Push"))
            {
                this.entityPosition.Y += 47;
                this.isPushing = false;
                this.notPatrolling = false;
            }
            if (animation.Contains("Die"))
                this.Dead = true;
        }

    }
}