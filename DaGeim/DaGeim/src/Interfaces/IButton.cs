namespace RobotBoy.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public interface IButton
    {
        bool IsSelected { get; }

        Rectangle Location { get; }

        void Load(ContentManager content);

        void DrawButton(SpriteBatch spriteBatch, SpriteFont font);
    }
}
