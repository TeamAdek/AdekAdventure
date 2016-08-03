using Microsoft.Xna.Framework;
using RobotBoy.Entities;
using RobotBoy.Entities.Ammunition;

namespace RobotBoy.Interfaces
{
    interface ICollidable
    {
        Rectangle CollisionBox { get; }
        void UpdateCollisionBounds();
        void CollisionWithMap(Rectangle collisionBox, int mapWidth, int mapHeight);
        void CollisionWithEntity(Entity entity);
        void CollisionWithAmmunition(Ammunition ammunition);
    }
}
