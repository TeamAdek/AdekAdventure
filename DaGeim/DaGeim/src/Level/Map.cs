namespace DaGeim.Level
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Map 
        {
        private int tileSize;
        private List<CollisionTiles> collisionTiles = new List<CollisionTiles>();
        private Texture2D backText;
        private Rectangle backRect;
        private int yPosition = -50;

        public List<CollisionTiles> CollisionTiles => this.collisionTiles;
        private int widht, height;

        public int TileSize
        {
            get { return this.tileSize; }
            set { this.tileSize = value; }
        }
        public void  Update(Vector2 position)
        {
            int calculated = (int)position.Y / 30 - 50;
            this.backRect.Y = calculated;
        }
        public int Widht
        {
            get { return this.widht; }
        }
        public int Height
        {
            get { return this.height; }
        }

        public Map()
        {
            this.TileSize = 72; // size of single tiile
        }
        public void Load(Map map , ContentManager Content )
        {
            this.backRect = new Rectangle(0,  0,6000, 700);
            this.backText = Content.Load<Texture2D>("background");
            map.Generate(new int[,]{

                      { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
                      { 6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
                      { 6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
                      { 6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5,5,5,5,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
                      { 6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,3,0,0,0,0,0,5,5,5,5,5,5,5,5,5,0,0,0,0,0,1,2,2,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
                      { 6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,5,5,5,3,0,0,0,0,0,5,5,5,5,5,5,5,0,0,0,0,0,1,5,5,5,5,5,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
                      { 6,0,0,0,0,0,0,0,0,1,2,3,0,0,1,2,3,0,0,0,0,0,1,2,2,2,2,2,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,5,5,5,5,5,5,5,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
                      { 6,0,0,0,1,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,5,5,5,5,5,5,5,5,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,5,5,5,5,5,5,5,5,5,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
                      { 5,2,2,2,5,5,5,3,0,0,0,0,0,0,0,0,0,0,1,2,5,5,5,5,5,5,5,5,5,5,5,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,5,5,5,5,5,5,5,5,5,5,5,2,2,2,2,2,2,2,2,2,2,2,2,2,2,5},
                      { 5,5,5,5,5,5,5,5,2,2,2,2,2,2,2,2,2,2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5}, //10

               }, this.tileSize);

        }
        
        public void Generate(int[,] map, int size)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    int number = map[y, x];

                    if (number > 0) this.collisionTiles.Add(new CollisionTiles(number, new Rectangle(x * size, y * size, size, size)));

                    this.widht = (x + 1) * size;
                    this.height = (y + 1) * size;
                }
            }

            this.collisionTiles.Sort();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.backText, this.backRect, Color.White);
            foreach (CollisionTiles tile in this.collisionTiles)
                tile.Draw(spriteBatch);
        }
    }
}
