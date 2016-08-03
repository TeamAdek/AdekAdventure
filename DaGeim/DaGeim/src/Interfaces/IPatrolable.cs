using Microsoft.Xna.Framework;

namespace RobotBoy.Interfaces
{
    public interface IPatrolable
    {
        Vector2 Position { get; set; }

        Vector2 StartPoint { get; set; }

        int PatrolRange { get; set; }

        void Patrol();

        void PlayAnimation(string name);
    }
}