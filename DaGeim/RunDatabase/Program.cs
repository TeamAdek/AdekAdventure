using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RobotBoy.Data;
using RobotBoy.Entities.Player;
using RobotBoy.Game;

namespace RunDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            RobotBoyContext context = new RobotBoyContext();
            context.Players.Add(new Player(new Vector2(150, 465))
            {
                Health = 300,
                JumpBoostTimer = 0,
                Score = 0,

            });

            context.SaveChanges();

              // using (var game = new MainGame())
            
                //   game.Run();

            RobotBoy.Game.Launcher.Main();
        }
    }
}
