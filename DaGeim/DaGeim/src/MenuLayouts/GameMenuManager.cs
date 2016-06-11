//class that we use to turn off the unused the menus in the game
namespace DaGeim
{

    public static class GameMenuManager
    {
        public static bool mainMenuOn;
        public static bool pauseMenuOn;
        public static bool endGameMenuOn;
        public static bool gameOn;

        // the method below is used turn off the menus we dont need on screen right now
        // it is called in the update method of each menu
        public static void TurnOtherMenusOff()
        {
            if (mainMenuOn)
            {
                pauseMenuOn = false;
                endGameMenuOn = false;
                gameOn = false;
            }
            else if (pauseMenuOn)
            {
                mainMenuOn = false;
                endGameMenuOn = false;
                gameOn = false;
            }
            else if (endGameMenuOn)
            {
                mainMenuOn = false;
                pauseMenuOn = false;
                gameOn = false;
            }
            else if (gameOn)
            {
                mainMenuOn = false;
                pauseMenuOn = false;
                endGameMenuOn = false;
            }
        }

    }
}
