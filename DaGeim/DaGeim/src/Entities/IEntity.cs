namespace Game.src.Entities
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IEntity
    {
        /// <summary>
        /// Draw rectangle of entity
        /// </summary>
        Rectangle CollisionBox { get; set; }

        /// <summary>
        /// Draw metod of entity
        /// </summary>
        /// <param name="spriteBach"></param>
        void Draw(SpriteBatch spriteBach);

        /// <summary>
        /// Make collision with map
        /// </summary>
        /// <param name="tileRectangle"></param>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        void CollisionWithMap(Rectangle tileRectangle, int mapWidth, int mapHeight);
    }
}
