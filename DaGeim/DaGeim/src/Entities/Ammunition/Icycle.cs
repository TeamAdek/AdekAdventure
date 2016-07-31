﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DaGeim.Entities.Ammunition
{
    public class Icycle : Ammunition
    {
        private static Texture2D LeftTexture { get; set; }
        private static Texture2D RightTexture { get; set; }
        public Icycle(Vector2 position, string direction, Texture2D left, Texture2D right) :
            base(position, direction)
        {
            this.Load(left,right);
            this.Sprite = direction == "left" ? LeftTexture : RightTexture;
        }

        public override void LoadContent(ContentManager content)
        {
        }

        public void Load(Texture2D left, Texture2D right)
        {
            LeftTexture = left;
            RightTexture = right;
        }
    }
}
