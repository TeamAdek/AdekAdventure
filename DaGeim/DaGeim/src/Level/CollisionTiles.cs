namespace DaGeim.Level
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class CollisionTiles : Tiles, IComparable<CollisionTiles>
    {
        public CollisionTiles(int i, Rectangle newRectangle)
        {
            this.texture = Content.Load<Texture2D>("Tile" + i);
            this.Rectangle = newRectangle;
        }

        public int CompareTo(CollisionTiles other)
        {
            return this.Rectangle.X.CompareTo(other.Rectangle.X);
        }
    }
}
