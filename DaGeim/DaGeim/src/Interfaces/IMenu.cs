namespace RobotBoy.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Game;

    public interface IMenu
    {
        SpriteFont CreditsFont { get; }

        SpriteFont MainFont { get; }

        SpriteFont Font { get; }

        SpriteFont GameNameFont { get; }

        void Load(ContentManager content);

        void Update(GameTime gameTime, MainGame game);

        void Draw(SpriteBatch spriteBatch);
    }
}

