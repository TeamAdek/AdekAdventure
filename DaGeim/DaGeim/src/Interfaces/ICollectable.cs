﻿using System;
using Game.src.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DaGeim.Interfaces
{
    public interface ICollectable
    {
        string ItemType { get; }

        int RestoreHealthPoints { get; }

        int JumpBoost { get; }

        int BonusScorePoints { get; }

        Vector2 Position { get; set; }
        Rectangle CollisionBox { get; }

        void Load(ContentManager Content);

        void CollisionWithPlayer(Entity entity); // Set from IEntity to Entity
        
        void Update(GameTime gameTime);

        Rectangle setRectangle(int x, int y, int w, int h);
        
        void Draw(SpriteBatch spriteBatch);
        
    }
}