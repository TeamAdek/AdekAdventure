﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RobotBoy.Entities.Ammunition
{
    public class Rocket : Ammunition
    {

        private static Texture2D LeftTexture { get; set; }
        private static Texture2D RightTexture { get; set; }
       // public Rectangle CollisionBox { get; set; }


        public Rocket(Vector2 position, string direction, Texture2D left, Texture2D right)
            :base(position, direction)
        {
            Load(left, right);
            Sprite = direction == "left" ? LeftTexture : RightTexture;
        }

        [Required]
        [Column("Id")]
        public int Id { get; set; }
    
        public override void LoadContent(ContentManager content)
        {
//            LeftTexture = content.Load<Texture2D>("rocketLeft");
//            RightTexture = content.Load<Texture2D>("rocketRight");
        }

        public void Load(Texture2D left, Texture2D right)
        {
            LeftTexture = left;
            RightTexture = right;
        }
    }
}
