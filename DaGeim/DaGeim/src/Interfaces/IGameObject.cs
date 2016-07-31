namespace DaGeim.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public interface IGameObject
    {
        void LoadContent(ContentManager content);
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime);
    }
}
