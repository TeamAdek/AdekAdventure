//helper class used to select the buttons

namespace RobotBoy.Helper_Classes
{
    public class Selector
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Selector(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
