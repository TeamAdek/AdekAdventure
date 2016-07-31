namespace DaGeim.Helper_Classes
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Lasers
    {
        public Texture2D shootTexture;
        public Vector2 shootPosition;
        public bool isVisible;
        public float speed;
        public string direction;

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Rocket Constructor
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public Lasers(Texture2D newBullet)
        {
            this.speed = 10;
            this.shootTexture = newBullet;
            this.isVisible = false;
            this.direction = "right";
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        // Draw rockets
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.shootTexture, this.shootPosition, Color.White);
        }

        public void Collision(Rectangle otherObject)
        {
            Rectangle thisRectangle = new Rectangle((int)this.shootPosition.X, (int)this.shootPosition.Y, this.shootTexture.Width, this.shootTexture.Height);
            if (thisRectangle.Intersects(otherObject))
            {
                this.isVisible = false;
            }
        }

        public Rectangle getCollisionBox()
        {
            return new Rectangle((int)this.shootPosition.X, (int)this.shootPosition.Y, this.shootTexture.Width, this.shootTexture.Height);
        }
    }
}