using DaGeim;
using DaGeim.src.Entities.New_Code;
using Game.src.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public abstract class Entity : AnimatedSprite, IGameObject, ICollidable
{
    /// <summary>
    /// General Entity Properties.
    /// </summary>
    protected Texture2D entityTexture; // The entity image that is currently rendered on screen.
    protected Vector2 entityPosition;  // The entity position.
    protected Vector2 entityVelocity; // Current entity speed.
    protected Vector2 entityDirection; // Determines how much [-1..0..1] should an entity move on the x and y axis.
    protected Orientations entityOrientation; // Determines which way is the enemy facing currently. up,down,left,right

    public Rectangle CollisionBox { get; protected set; }
    public int Health { get;  set; }
    public bool Dead { get; set; }

    protected enum Orientations{Left,Right,Up,Down}
    
    protected Entity(Vector2 position)
    {
        entityPosition = position;
    }

    public Vector2 Position { get { return this.entityPosition; } }

    public abstract void LoadContent(ContentManager content);
    public abstract void Draw(SpriteBatch spriteBatch);
    protected abstract void LoadAnimations();

    protected Rectangle SetCollisionRectangle(int x, int y, int w, int h)
    {
        return new Rectangle((int)entityPosition.X + x, (int)entityPosition.Y + y, w, h);
    }

    public virtual void CollisionWithMap(Rectangle tileRectangle, int mapWidth, int mapHeight)
    {
         if (CollisionBox.TouchTopOf(tileRectangle))
        { 
            CollisionBox = new Rectangle(CollisionBox.X, tileRectangle.Y - CollisionBox.Height, CollisionBox.Width, CollisionBox.Height);
            entityVelocity.Y = 0.0f;
            entityDirection.Y = 0.0f;
        }
        if (CollisionBox.TouchLeftOf(tileRectangle))
            entityPosition.X = tileRectangle.X - tileRectangle.Width - 2;

        if (CollisionBox.TouchRightOf(tileRectangle))
            entityPosition.X = tileRectangle.X + tileRectangle.Width + 2;

        if (CollisionBox.TouchBottomOf(tileRectangle))
            entityVelocity.Y = 1.0f;

        if (entityPosition.X < 0.0f) entityPosition.X = 0.0f;
        if (entityPosition.X > mapWidth - CollisionBox.Width) entityPosition.X = mapWidth - CollisionBox.Width;
        if (entityPosition.Y < 0.0f) entityVelocity.Y = 1.0f;
        if (entityPosition.Y > mapHeight - CollisionBox.Height) entityPosition.Y = mapHeight - CollisionBox.Height;
    }

    public virtual void CollisionWithEntity(Entity entity)
    {
        if (this.CollisionBox.Intersects(entity.CollisionBox))
        {
            ///TODO: Add Collision Logic
        }
    }
    public virtual void CollisionWithAmmunition(Ammunition ammunition)
    {
        if (CollisionBox.Intersects(ammunition.CollisionBox))
        {
            Health -= 50;

            if (Health <= 0)
                Dead = true;

            ammunition.IsVisible = false;
        }
    }

    public abstract void UpdateCollisionBounds();
}
