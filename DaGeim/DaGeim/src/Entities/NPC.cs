namespace DaGeim.Entities
{
    using System;
    using System.Collections.Generic;
    using DaGeim.Entities.Ammunition;
    using DaGeim.Helper_Classes;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class NPC : Entity
    {
        private Vector2 startPosition;
        public List<Ammunition.Ammunition> ammo;
        protected Type ammoType;
        private int patrolRange;
        protected Texture2D ammoLeft, ammoRight;
        protected float shootCD;
        public bool IsAttacking { get; protected set; }
        protected bool notPatrolling, hasCollidedLeft, hasCollidedRight, isDying;
        public NPC(Vector2 position, int patrolRange) : base(position)
        {
            this.startPosition = position;
            this.patrolRange = patrolRange;
            this.ammo = new List<Ammunition.Ammunition>();
        }

        protected virtual void Patrol()
        {
            if (!this.notPatrolling)
            {
                if (this.hasCollidedLeft || this.hasCollidedRight)
                {
                    this.patrolRange = (int) Math.Abs(this.startPosition.X - this.entityPosition.X);

                    if (this.entityOrientation == Orientations.Left && this.hasCollidedRight)
                    {
                        this.PlayAnimation("WalkRight");
                        this.entityOrientation = Orientations.Right;
                    }
                    else if (this.entityOrientation == Orientations.Right && this.hasCollidedLeft)
                    {
                        this.PlayAnimation("WalkLeft");
                        this.entityOrientation = Orientations.Left;
                    }
                    this.hasCollidedLeft = false;
                    this.hasCollidedRight = false;
                }

                if ((this.entityOrientation == Orientations.Left) && (this.entityPosition.X > this.startPosition.X - this.patrolRange))
                {
                    this.entityPosition.X -= 2;
                    this.PlayAnimation("WalkLeft");
                }

                if ((this.entityOrientation == Orientations.Left) && ((this.entityPosition.X <= this.startPosition.X - this.patrolRange)))
                {
                    this.PlayAnimation("WalkRight");
                    this.entityOrientation = Orientations.Right;
                }

                if ((this.entityOrientation == Orientations.Right) && (this.entityPosition.X < this.startPosition.X + this.patrolRange))
                {
                    this.PlayAnimation("WalkRight");
                    this.entityPosition.X += 2;
                }

                if ((this.entityOrientation == Orientations.Right) && ((this.entityPosition.X >= this.startPosition.X + this.patrolRange)))
                {
                    this.PlayAnimation("WalkLeft");
                    this.entityOrientation = Orientations.Left;
                }
            }
        }

        public override void CollisionWithMap(Rectangle tileRectangle, int mapWidth, int mapHeight)
        {
            if (this.CollisionBox.TouchLeftOf(tileRectangle))
                this.hasCollidedLeft = true;
            if (this.CollisionBox.TouchRightOf(tileRectangle))
                this.hasCollidedRight = true;

            base.CollisionWithMap(tileRectangle, mapWidth, mapHeight);
        }

        protected virtual void Chase(Player.Player player)
        {

        }

        public virtual void Shoot(Player.Player player)
        {
            if (this.PlayerIsInRange(player) && !this.isDying)
            {
                if (this.shootCD > 0.0f)
                    this.shootCD--;
                if (this.shootCD.Equals(0.0f))
                {
                    if (player.Position.X < this.entityPosition.X)
                        this.entityOrientation = Orientations.Left;
                    else if (player.Position.X >= this.entityPosition.X)
                        this.entityOrientation = Orientations.Right;

                    Ammunition.Ammunition currentAmmo;
                    switch (this.ammoType.FullName)
                    {
                        case "Laser":
                            currentAmmo = new Laser(new Vector2(this.entityPosition.X -15, this.entityPosition.Y + 35), "left", this.ammoLeft);
                            break;
                        case "Icycle":
                            if (this.entityOrientation == Orientations.Left)
                                currentAmmo = new Icycle(new Vector2(this.entityPosition.X - 15, this.entityPosition.Y + 35), "left", this.ammoLeft,this.ammoRight);
                            else
                                currentAmmo = new Icycle(new Vector2(this.entityPosition.X + 80, this.entityPosition.Y + 35), "right", this.ammoLeft, this.ammoRight);
                            break;
                        default:currentAmmo = null;break;
                    }

                    if (this.entityOrientation == Orientations.Left)
                    {
                        this.PlayAnimation("ShootLeft");
                        this.IsAttacking = true;
                    }
                    else
                    {
                        this.PlayAnimation("ShootRight");
                        this.IsAttacking = true;
                    }

                    this.ammo.Add(currentAmmo);
                    this.shootCD = 40.0f;
                }
            }
        }

        protected virtual void UpdateAmmo(GameTime gameTime)
        {
            foreach (var ammunition in this.ammo)
                ammunition.Update(gameTime);

            for (int i = 0; i < this.ammo.Count; i++)
            {
                if (!this.ammo[i].IsVisible)
                {
                    this.ammo.RemoveAt(i);
                    i--;
                }
            }
        }

        protected bool PlayerIsInRange(Player.Player player)
        {
            float lowBound = player.Position.Y - 35;
            float highBound = player.Position.Y + 35;
            int range = 600;
            float positionDelta = Math.Abs(player.Position.X - this.entityPosition.X);

            if (this.entityPosition.Y > lowBound && this.entityPosition.Y < highBound)
            {
                if (positionDelta <= range)
                    return true;
            }

            return false;
        }

        public override void CollisionWithAmmunition(Ammunition.Ammunition ammunition)
        {
            if (this.CollisionBox.Intersects(ammunition.CollisionBox))
            {
                this.Health -= 50;

                if (this.Health <= 0)
                {
                    if(this.entityOrientation == Orientations.Left)
                        this.PlayAnimation("DieLeft");
                    else
                        this.PlayAnimation("DieRight");

                    this.isDying = true;
                }

                ammunition.IsVisible = false;
            }
        }
    }
}
