using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

class HUD
{
    private Texture2D HUDTexture;
    private Texture2D HPBar;

    private Rectangle healthBar = new Rectangle(0, 0, 1280, 720);
    public void Load(ContentManager content)
    {
        HUDTexture = content.Load<Texture2D>("HUD");
        HPBar = content.Load<Texture2D>("HUD_HP_BAR");
    }

    public void Update(int hp)
    {
        healthBar.Width = hp;
    }
    public void Draw(SpriteBatch renderEngine)
    {
        renderEngine.Draw(HPBar, new Vector2(0, 25), healthBar, Color.AliceBlue);
        renderEngine.Draw(HUDTexture, new Vector2(0, 25), new Rectangle(0, 0, 1280, 720), Color.AliceBlue);
    }
}
