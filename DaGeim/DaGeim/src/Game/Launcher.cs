namespace DaGeim.Game
{
    using System;

#if WINDOWS || LINUX
    public static class Launcher
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new MainGame())
                game.Run();
        }
    }
#endif
}
