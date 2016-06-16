using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace DaGeim
{
    class Tiles
    {
        protected Texture2D texture;
        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
            protected set { rectangle = value; }
        }

        private static ContentManager content;
        public static ContentManager Content
        {
            protected get { return content; }
            set { content = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);

        }
    }

    class CollisionTiles : Tiles, IComparable<CollisionTiles>
    {
        public CollisionTiles(int i, Rectangle newRectangle)
        {
            texture = Content.Load<Texture2D>("Tile" + i);
            this.Rectangle = newRectangle;
        }

        public int CompareTo(CollisionTiles other)
        {
            return this.Rectangle.X.CompareTo(other.Rectangle.X);
        }
    }
}
