namespace DaGeim.src.Interfaces
{
    using Game;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public interface IMenu
    {

        SpriteFont CreditsFont{get;}

         SpriteFont MainFont { get; }

         SpriteFont Font { get; }

         SpriteFont GameNameFont { get; }

        void Load(ContentManager content);

        void Update(GameTime gameTime, MainGame game);

        void Draw(SpriteBatch spriteBatch);
    }
}
