namespace DaGeim.Level
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Camera
    {
        private Matrix transform;
        public Matrix Transform
        {
            get { return this.transform; }
        }
        public static Vector2 centre;
        private Viewport viewport;

        public Camera(Viewport newViewport)
        {
            this.viewport = newViewport;
        }

        public void Update (Vector2 position, int xOffset, int yOffset)
        {
            if (position.X < this.viewport.Width / 2)
                centre.X = this.viewport.Width / 2;            
            else if (position.X > xOffset - (this.viewport.Width / 2))
                    centre.X = xOffset - (this.viewport.Width / 2);
                else centre.X = position.X;

            if (position.Y < this.viewport.Height / 2)
                centre.Y = this.viewport.Height / 2;
            else if (position.Y > yOffset - (this.viewport.Height / 2))
                centre.Y = yOffset - (this.viewport.Height / 2);
            else centre.Y = position.Y;

            this.transform = Matrix.CreateTranslation(new Vector3(-centre.X + (this.viewport.Width / 2),-centre.Y + (this.viewport.Height/2) ,0));

        }
                
    }
}
