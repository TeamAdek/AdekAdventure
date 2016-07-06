namespace DaGeim.Interfaces
{
    /// <summary>
    /// every character that can collect jumpBoosting items mast implement this interface
    /// he mast has a timer to count the estimating time of jumpBoost and
    /// a function to check the time.
    /// </summary>
    public interface IJumpboostable
    {
        int JumpBoostTimer { get; set; }

        void JumpBoostCheckTimer();
    }
}