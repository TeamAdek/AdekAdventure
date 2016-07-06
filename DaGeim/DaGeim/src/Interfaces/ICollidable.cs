using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace DaGeim.src.Entities.New_Code
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
