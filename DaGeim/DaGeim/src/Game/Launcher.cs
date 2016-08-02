using System;

namespace DaGeim.Game
{
#if WINDOWS || LINUX
    public static class Launcher
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MainGame())
                game.Run();
        }
    }
#endif
}
