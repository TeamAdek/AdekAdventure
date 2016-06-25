using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DaGeim.src.Collectable
{
    class HPBonus
    {
        private Texture2D spriteTexture;
        private Vector2 position;

        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        public HPBonus(Vector2 position)
        {
           // this.spriteTexture = spriteTexture;
            this.position = position;
        }

        public void Load(ContentManager Content)
        {
            spriteTexture = Content.Load<Texture2D>("enemyRobbot");
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D t = spriteTexture;
            Vector2 v = Position;
            spriteBatch.Draw(t, v, Color.White);
        }
    }
}
