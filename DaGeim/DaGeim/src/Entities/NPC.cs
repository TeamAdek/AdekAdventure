using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RobotBoy.Entities.Ammunition;
using RobotBoy.GUI;
using RobotBoy.Helper_Classes;

namespace RobotBoy.Entities
{
    internal abstract class NPC : Entity
    {
        private Vector2 startPosition;
        private EnemyHealthBar healthBar;
        public List<Ammunition.Ammunition> ammo;
        protected Type ammoType;
        private int patrolRange;
        protected Texture2D ammoLeft, ammoRight;
        protected float shootCD;
        public bool IsAttacking { get; protected set; }
        protected bool notPatrolling, hasCollidedLeft, hasCollidedRight, isDying;
        protected NPC(Vector2 position, int patrolRange) : base(position)
        {
            startPosition = position;
            healthBar = new EnemyHealthBar();
            this.patrolRange = patrolRange;
            ammo = new List<Ammunition.Ammunition>();
        }

        public override void LoadContent(ContentManager content)
        {
            healthBar.Load(content);
        }

        public override void Update(GameTime gameTime)
        {
            healthBar.Update(this.Health, entityPosition);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            healthBar.Draw(spriteBatch);
        }

        protected virtual void Patrol()
        {
            if (!notPatrolling)
            {
                if (hasCollidedLeft || hasCollidedRight)
                {
                    patrolRange = (int) Math.Abs(startPosition.X - entityPosition.X);

                    if (entityOrientation == Orientations.Left && hasCollidedRight)
                    {
                        PlayAnimation("WalkRight");
                        entityOrientation = Orientations.Right;
                    }
                    else if (entityOrientation == Orientations.Right && hasCollidedLeft)
                    {
                        PlayAnimation("WalkLeft");
                        entityOrientation = Orientations.Left;
                    }
                    hasCollidedLeft = false;
                    hasCollidedRight = false;
                }

                if ((entityOrientation == Orientations.Left) && (entityPosition.X > startPosition.X - patrolRange))
                {
                    entityPosition.X -= 2;
                    PlayAnimation("WalkLeft");
                }

                if ((entityOrientation == Orientations.Left) && ((entityPosition.X <= startPosition.X - patrolRange)))
                {
                    PlayAnimation("WalkRight");
                    entityOrientation = Orientations.Right;
                }

                if ((entityOrientation == Orientations.Right) && (entityPosition.X < startPosition.X + patrolRange))
                {
                    PlayAnimation("WalkRight");
                    entityPosition.X += 2;
                }

                if ((entityOrientation == Orientations.Right) && ((entityPosition.X >= startPosition.X + patrolRange)))
                {
                    PlayAnimation("WalkLeft");
                    entityOrientation = Orientations.Left;
                }


            }

        }


        public override void CollisionWithMap(Rectangle tileRectangle, int mapWidth, int mapHeight)
        {
            if (CollisionBox.TouchLeftOf(tileRectangle))
                hasCollidedLeft = true;
            if (CollisionBox.TouchRightOf(tileRectangle))
                hasCollidedRight = true;

            base.CollisionWithMap(tileRectangle, mapWidth, mapHeight);
        }

        protected virtual void Chase(Player.Player player)
        {

        }

        public virtual void Shoot(Player.Player player)
        {
            if (PlayerIsInRange(player) && !isDying)
            {
                if (shootCD > 0.0f)
                    shootCD--;
                if (shootCD == 0.0f)
                {
                    if (player.Position.X < entityPosition.X)
                        entityOrientation = Orientations.Left;
                    else if (player.Position.X >= entityPosition.X)
                        entityOrientation = Orientations.Right;

                    Ammunition.Ammunition currentAmmo;
                    switch (ammoType.FullName)
                    {
                        case "Laser":
                            currentAmmo = new Laser(new Vector2(entityPosition.X -15, entityPosition.Y + 35), "left", ammoLeft);
                            break;
                        case "Icycle":
                            if (entityOrientation == Orientations.Left)
                                currentAmmo = new Icycle(new Vector2(entityPosition.X - 15, entityPosition.Y + 35), "left", ammoLeft,ammoRight);
                            else
                                currentAmmo = new Icycle(new Vector2(entityPosition.X + 80, entityPosition.Y + 35), "right", ammoLeft, ammoRight);
                            break;
                        default:currentAmmo = null;break;
                    }

                    if (entityOrientation == Orientations.Left)
                    {
                        PlayAnimation("ShootLeft");
                        IsAttacking = true;
                    }
                    else
                    {
                        PlayAnimation("ShootRight");
                        IsAttacking = true;
                    }

                    ammo.Add(currentAmmo);
                    shootCD = 40.0f;
                }
            }

        }

        protected virtual void UpdateAmmo(GameTime gameTime)
        {
            foreach (var ammunition in ammo)
                ammunition.Update(gameTime);

            for (int i = 0; i < ammo.Count; i++)
            {
                if (!ammo[i].IsVisible)
                {
                    ammo.RemoveAt(i);
                    i--;
                }
            }
        }

        protected bool PlayerIsInRange(Player.Player player)
        {
            float lowBound = player.Position.Y - 35;
            float highBound = player.Position.Y + 35;
            int range = 600;
            float positionDelta = Math.Abs(player.Position.X - entityPosition.X);

            if (entityPosition.Y > lowBound && entityPosition.Y < highBound)
            {
                if (positionDelta <= range)
                    return true;
            }

            return false;
        }

        public override void CollisionWithAmmunition(Ammunition.Ammunition ammunition)
        {
            if (CollisionBox.Intersects(ammunition.CollisionBox))
            {
                Health -= 50;

                if (Health <= 0)
                {
                    if(entityOrientation == Orientations.Left)
                        PlayAnimation("DieLeft");
                    else
                        PlayAnimation("DieRight");

                    isDying = true;
                }

                ammunition.IsVisible = false;
            }
        }
    }
}
