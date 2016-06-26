using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DaGeim.Interfaces;
using Game.src.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DaGeim.src.Collectable
{
    class HealthRestore : CollectableItem
    {
        

        public HealthRestore(Vector2 position, string itemType = "healthRestore") : base(position)
        {
            this.position = position;
            this.itemType = itemType;
            this.restoreHealthPoints = 50;
        }
        
        public override void Load(ContentManager content)
        {
            //spriteTexture = Content.Load<Texture2D>("heart");
            //spriteTexture = Content.Load<Texture2D>("pow");
            spriteTexture = content.Load<Texture2D>("Corazon");
        }
    }
}
