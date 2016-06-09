using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    class Layer
    {
        // dimentions of map - in tiles:
        int m_Height;
        public int Height
        {
            get
            {
                return m_Height;
            }
        }

        int m_Width;
        public int Width
        {
            get
            {
                return m_Width;
            }
        }

        // dimentions of tile - in pixels:
        int m_TileWidth;
        int m_TileHeight;

        // texture:
        Texture2D m_TilesTexture;

        int[,] tilesInMap;


        public void Initialize(Texture2D texture, int width, int height, int tileWidth, int tileHeight)
        {
            m_TilesTexture = texture;
            m_Height = height;
            m_Width = width;
            m_TileWidth = tileHeight;
            m_TileHeight = tileWidth;

            tilesInMap = new int[width,height];
        }

        public void Load(string fileName)
        {
            // Get the file's text.
            string whole_file = System.IO.File.ReadAllText(fileName);

            // Split into lines.
            whole_file = whole_file.Replace('\n', '\r');
            string[] lines = whole_file.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            // See how many rows and columns there are.
            m_Height = lines.Length;
            m_Width = lines[0].Split(',').Length;

            tilesInMap = new int[m_Width, m_Height];

            // Load the array.
            for (int r = 0; r < m_Height; r++)
            {
                string[] line_r = lines[r].Split(',');
                for (int c = 0; c < m_Width; c++)
                {
                    tilesInMap[c, r] = int.Parse(line_r[c]);
                }
            }                   
        }
    

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, int cameraPosX, int cameraPosY, int screenWidth, int screenHeight)
        {
            int halfScrWidth = screenWidth / 2;
            int halfScrHeigth = screenHeight / 2;

            int borderUpLeftX = cameraPosX - halfScrWidth;
            int borderUpLeftY = cameraPosY + halfScrHeigth;
            int borderDownRightX = cameraPosX + halfScrWidth;
            int borderDownRightY = cameraPosY - halfScrHeigth;

            // points UpLeft and DownRight to tiles:
            borderUpLeftX /= m_TileWidth;
            borderUpLeftX = Math.Max(0, borderUpLeftX);

            borderUpLeftY /= m_TileHeight;
            borderUpLeftY = Math.Min(m_Height - 1, borderUpLeftY);

            borderDownRightX /= m_TileWidth;
            borderDownRightX = Math.Min(m_Width - 1, borderDownRightX);

            borderDownRightY /= m_TileHeight;
            borderDownRightY = Math.Max(0, borderDownRightY);
                            
            // width of texture in tiles:
            int textureTileWidth = m_TilesTexture.Width / m_TileWidth;

            int startX = borderUpLeftX * m_TileWidth - (cameraPosX - halfScrWidth);
            int startY = (cameraPosY + halfScrHeigth - (borderUpLeftY+1) * m_TileHeight);
            int currentY = startY; 

            for (int i = borderUpLeftX; i <= borderDownRightX; i++)
            {
                //for (int j = borderDownRightY; j <= borderUpLeftY; j++)
                for (int j = borderUpLeftY; j >= borderDownRightY; j--)
                {
                    int tileIdx = tilesInMap[i, m_Height - 1 - j];
                    if (tileIdx >= 0)
                    {
                        //to pixels:
                        int tBorderUpLeftX = (tileIdx % textureTileWidth) * m_TileWidth;
                        int tBorderUpLeftY = (tileIdx / textureTileWidth) * m_TileHeight;
                        spriteBatch.Draw(m_TilesTexture, new Rectangle(startX, currentY, m_TileWidth, m_TileHeight), new Rectangle(tBorderUpLeftX, tBorderUpLeftY, m_TileWidth, m_TileHeight), Color.White);
                    }
                    currentY += m_TileHeight;
                }
                currentY = startY;
                startX += m_TileWidth;
            }
        }

        public Rectangle[] GetCollision(ref Rectangle rect)
        {
            List<Rectangle> m_CollisionRects = new List<Rectangle>();
            int borderUpLeftX = rect.X / m_TileWidth;
            int borderUpLeftY = rect.Y / m_TileHeight;
            int borderDownRightX = borderUpLeftX + rect.Width / m_TileWidth;
            int borderDownRightY = borderUpLeftY + rect.Height / m_TileHeight;

            borderUpLeftX = Math.Max(0, borderUpLeftX);
            borderUpLeftY = Math.Min(0, borderUpLeftY);

            borderDownRightX = Math.Min(m_Width - 1, borderDownRightX);
            borderDownRightY = Math.Max(m_Height - 1, borderDownRightY);


            for (int i = borderUpLeftX; i <= borderDownRightX; i++)
            {
                //for (int j = borderDownRightY; j <= borderUpLeftY; j++)
                for (int j = borderUpLeftY; j <= borderDownRightY; j--)
                {
                    if (tilesInMap[i, j] != -1)
                    {
                        m_CollisionRects.Add(new Rectangle(i * m_TileWidth, j * m_TileHeight, m_TileWidth, m_TileHeight));
                    }
                }
            }
            return m_CollisionRects.ToArray();
        }
    }
}
