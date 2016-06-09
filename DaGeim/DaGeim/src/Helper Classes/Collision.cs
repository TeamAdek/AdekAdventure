using System;
using Microsoft.Xna.Framework;

static class Collision
{
    public static bool checkCollision(Rectangle objectA, Rectangle objectB)
    {
        if (objectA.Intersects(objectB))
            return true;

        return false;
    }
}
