﻿namespace DaGeim.MenuLayouts
{
    public static class GameMenuManager
    {
        public static bool mainMenuOn = true;
        public static bool pauseMenuOn;
        public static bool endGameMenuOn;
        public static bool gameOn;
        public static bool creditsMenuOn;

        /// <summary>
        /// turn off the menus we dont need on screen right now;
        /// it is called in the update method of each menu
        /// </summary>
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

        
    }
}
