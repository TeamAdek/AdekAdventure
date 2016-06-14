using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Rockets
{
    public Texture2D shootTexture;
    public Vector2 shootPosition;
    public bool isVisible;
    public float speed;
    public string direction;

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    // Rocket Constructor
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------

    public Rockets(Texture2D newBullet)
    {
        speed = 5;
        shootTexture = newBullet;
        isVisible = false;
        direction = "right";
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------
    // Draw rockets
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(shootTexture, shootPosition, Color.White);
    }

    public void Collision(Rectangle otherObject)
    {
        Rectangle thisRectangle = new Rectangle((int)this.shootPosition.X, (int)this.shootPosition.Y, this.shootTexture.Width, this.shootTexture.Height);
        if (thisRectangle.Intersects(otherObject))
        {
            this.isVisible = false;
        }
    }
}