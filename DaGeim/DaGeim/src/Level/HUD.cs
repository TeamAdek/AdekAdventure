using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

class HUD
{
    private Texture2D HUDTexture;
    private Texture2D HPBar;
    public Vector2 healthPosition;

    private Rectangle healthBar = new Rectangle(25, 0, 1280, 720);
    private Rectangle health = new Rectangle(0, 0, 1280, 720);
    public void Load(ContentManager content)
    {
        HUDTexture = content.Load<Texture2D>("HUD");
        HPBar = content.Load<Texture2D>("HUD_HP_BAR");
    }

    public void Update(int hp , Vector2 newPosition)
    {
        healthBar.Width = hp;
        healthPosition.X = newPosition.X - (1280 / 2); // game width / 2
        healthPosition.Y = newPosition.Y - (720 / 2); // game height / 2

    }
    public void Draw(SpriteBatch renderEngine)
    {
        renderEngine.Draw(HPBar, healthPosition, healthBar, Color.AliceBlue);
        renderEngine.Draw(HUDTexture, healthPosition, health, Color.AliceBlue);
    }
}
