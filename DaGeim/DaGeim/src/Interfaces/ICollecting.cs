namespace DaGeim.Interfaces
{
    /// <summary>
    /// Every character who can collect items mast implement this interface.
    /// He mast has a CollisionWithCollectable function.
    /// </summary>
    interface ICollecting
    {
        void CollisionWithCollectable(ICollectable collectable);
    }
}
