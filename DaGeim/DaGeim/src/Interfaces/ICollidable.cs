namespace DaGeim.Interfaces
{
    using DaGeim.Entities;
    using DaGeim.Entities.Ammunition;
    using Microsoft.Xna.Framework;

    public interface ICollidable
    {
        Rectangle CollisionBox { get; }
        void UpdateCollisionBounds();
        void CollisionWithMap(Rectangle collisionBox, int mapWidth, int mapHeight);
        void CollisionWithEntity(Entity entity);
        void CollisionWithAmmunition(Ammunition ammunition);
    }
}
