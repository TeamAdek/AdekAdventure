using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RobotBoy.Entities.Ammunition;

namespace RobotBoy.Entities.Bosses
{
    internal sealed class Boss_L1 : NPC
    {

        public bool isPushing = false;

        public Boss_L1(Vector2 position, int range): base(position, range)
        {
            Health = 180;
            FramesPerSecond = 15;
            ammoType = typeof(Laser);
            LoadAnimations();
            PlayAnimation("WalkRight");
            entityOrientation = Orientations.Right;
        }

        public override void LoadContent(ContentManager content)
        {
            entityTexture = content.Load<Texture2D>("SpriteSheets/Boss");
            ammoLeft = content.Load<Texture2D>("Ammunition/Laser");
            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateAmmo(gameTime);

            if (!isDying)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                //entityPosition += entityVelocity;
                UpdateCollisionBounds();

                if (entityVelocity.Y < 10) /// used as gravity
                    entityVelocity.Y += 0.4f;

                if (!IsAttacking)
                    Patrol();
            }

            base.Update(gameTime);
        }

        public override void Shoot(Player.Player player)
        {
            int distanceToPlayer = (int)entityPosition.X - (int)player.Position.X;

            if (distanceToPlayer < 100)
            {
                PlayAnimation("Push");
                if (!isPushing)
                    entityPosition.Y -= 47;
                notPatrolling = true;
                isPushing = true;
            }

            base.Shoot(player);
        }

        public override void UpdateCollisionBounds()
        {
            Rectangle output = new Rectangle();
            switch (currAnimationSet)
            {
                case "WalkLeft": output = SetCollisionRectangle(30, 0, 45, 123); break;
                case "WalkRight": output = SetCollisionRectangle(30, 0, 45, 123); break;
                case "ShootLeft": output = SetCollisionRectangle(30, 0, 45, 123); break;
                case "Push": output = SetCollisionRectangle(30, 0, 45, 123); break;
                default: output = SetCollisionRectangle(30, 0, 45, 123); break;
            }
            CollisionBox = output;
        }

        protected override void LoadAnimations()
        {
            textureWidth = 128;
            textureHeight = 128;
            AddAnimation("WalkLeft", 12, 0);
            AddAnimation("WalkRight", 12, 1);
            AddAnimation("ShootLeft", 12, 2);
            AddAnimation("DieLeft", 12, 3);
            AddAnimation("DieRight", 12, 3);
            offSetYPx += 47;
            AddAnimation("Push", 12, 4);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var laser in ammo)
                laser.Draw(spriteBatch);

            base.Draw(spriteBatch);
            base.Draw(spriteBatch, entityPosition, entityTexture);
        }

        protected override void AnimationDone(string animation)
        {
            if (animation.Contains("Shoot"))
                IsAttacking = false;
            if (animation.Contains("Push"))
            {
                entityPosition.Y += 47;
                isPushing = false;
                notPatrolling = false;
            }
            if (animation.Contains("Die"))
                Dead = true;
        }

    }
}