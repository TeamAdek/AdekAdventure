//helper class used to store the scores in the scoreboard
namespace RobotBoy.Helper_Classes
{
    public class Score
    {
        public long points;
        public string playerName;

        public Score(string name, long points)
        {
            this.playerName = name;
            this.points = points;
        }
    }
}
