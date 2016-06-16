using DaGeim;

namespace Game.src.Entities
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IEntity
    {
        /// <summary>
        /// Draw rectangle of entity
        /// </summary>
        Rectangle CollisionBox { get; set; }

        /// <summary>
        /// Conteins active rockets shot by entity
        /// </summary>
        List<Rockets> Rockets { get; set; }

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

        /// <summary>
        /// Make collision with other entity
        /// </summary>
        /// <param name="entity"></param>
        void CollisionWithEntity(IEntity entity);

        /// <summary>
        /// Make collision with rocket
        /// </summary>
        /// <param name="rocket"></param>
        void CollisionWithRocket(Rockets rocket, Player player);
    }
}
