using System;
using System.Linq;
using Microsoft.Xna.Framework;
using RobotBoy.Context;
using RobotBoy.Entities.Player;

namespace RobotBoy.Game
{
#if WINDOWS || LINUX
    public static class Launcher
    {
        [STAThread]
        public static void Main()
        {
            //RobotBoyGameContext context = new RobotBoyGameContext();
            //if (!context.Players.Any())
            //{
            //    context.Players.Add(new Player(new Vector2(150, 465))
            //    {
            //        Health = 300,
            //        JumpBoostTimer = 0,
            //        Score = 0,

            //    });

            //    context.SaveChanges();
            //}


            using (var game = new MainGame())

                game.Run();
        }
    }
#endif
}
