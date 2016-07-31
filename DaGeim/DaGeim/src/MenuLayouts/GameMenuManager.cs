﻿//class that we use to turn off the unused menus in the game
namespace DaGeim.MenuLayouts
{
    using DaGeim.Entities.Player;
    using DaGeim.Level;
    using Microsoft.Xna.Framework;

    public static class GameMenuManager
    {
        public static bool mainMenuOn;
        public static bool pauseMenuOn;
        public static bool endGameMenuOn;
        public static bool gameOn;
        public static bool creditsMenuOn;

        /*-------------------------------------------------------------------------------------------------
        turn off the menus we dont need on screen right now;
        it is called in the update method of each menu
        ------------------------------------------------------------------------------------------------*/
        public static void TurnOtherMenusOff()
        {
            if (mainMenuOn)
            {
                pauseMenuOn = false;
                endGameMenuOn = false;
                gameOn = false;
            }
            if (pauseMenuOn)
            {
                mainMenuOn = false;
                endGameMenuOn = false;
                gameOn = false;
            }
            if (endGameMenuOn)
            {
                mainMenuOn = false;
                pauseMenuOn = false;
                gameOn = false;
            }
            if (gameOn)
            {
                mainMenuOn = false;
                pauseMenuOn = false;
                endGameMenuOn = false;
            }
            if (creditsMenuOn)
            {
                mainMenuOn = false;
                pauseMenuOn = false;
                gameOn = false;
                endGameMenuOn = false;
            }
        }

        public static void InitializeNewGame(Player player, Map map, HUD gameUI)
        {
            map = new Map();
            gameUI = new HUD();
            player = new Player(new Vector2(150, 465));
        }
    }
}
