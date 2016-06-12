using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DaGeim.Enemies;

namespace DaGeim
{
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Song song;
        private StartGameScreen startGameScreen;
        private EndGameScreen endGameScreen;
        private HUD gameUI;
        Texture2D backText;
        Rectangle backRect;

        Camera camera;
        Map map;

        Player mainPlayer;
        private Enemy1 enemy1;
        private EnemyGuardian enemy2;
        private EnemyGuardian enemy3;
        private List<EnemyGuardian> enemiesList = new List<EnemyGuardian>();
        private Texture2D enemy1Texture2D;

        public const int GAME_WIDTH = 1280;
        public const int GAME_HEIGHT = 720;

        public MainGame()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
                  
            graphics.PreferredBackBufferWidth = GAME_WIDTH;
            graphics.PreferredBackBufferHeight = GAME_HEIGHT;
            graphics.ApplyChanges();
            

        }

        protected override void Initialize()
        {
            //GameMenuManager.mainMenuOn = true; // we set the mainmenuON, because we want to start from the mainMenu
            startGameScreen = new StartGameScreen();
            endGameScreen = new EndGameScreen();

            map = new Map();
            gameUI = new HUD();

            mainPlayer = new Player(new Vector2(155, 325));

            enemy2 = new EnemyGuardian();
            enemy2.StartPoint = new Vector2(164, 380);
            enemy2.Position = enemy2.StartPoint;
            enemy3 = new EnemyGuardian();
            enemy3.StartPoint = new Vector2(300, 320);
            enemy3.Position = enemy3.StartPoint;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            startGameScreen.Load(Content);
            endGameScreen.Load(Content);

            camera = new Camera(GraphicsDevice.Viewport);

            Tiles.Content = Content;
            map.Generate(new int[,]{

                    { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    { 2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    { 2,1,0,0,0,0,0,0,0,0,0,0,1,1,1,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                    { 2,2,1,1,1,0,0,0,0,1,1,0,2,2,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0},
                    { 2,2,0,0,0,0,0,1,1,2,2,0,2,2,2,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,2,1,0,0,0,0,0,0},
                    { 2,0,0,0,0,0,1,2,2,2,2,0,2,2,2,2,2,1,1,1,1,1,1,0,0,0,0,0,0,0,1,2,1,1,1,2,2,2,1,1,1,1,1,1},
                    { 2,0,0,0,1,1,2,2,2,2,2,0,2,2,2,2,2,2,2,2,2,2,2,0,0,0,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
                    { 2,1,1,1,2,2,2,2,2,2,2,0,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
               }, 64);


            mainPlayer.LoadContent(Content);

            Texture2D enemyTexture2D = Content.Load<Texture2D>("enemy1");
            enemy1 = new Enemy1(enemyTexture2D, 2, 4);
            this.enemiesList.Add(this.enemy2);
            this.enemiesList.Add(this.enemy3);

            foreach (var enemy in enemiesList)
                enemy.Load(Content);

            DrawRect.LoadContent(Content);
            backText = Content.Load<Texture2D>("background");
            gameUI.Load(Content);
            backRect = new Rectangle(0, -50, 3000, 500);
            //loading the endGameScreen content
            endGameScreen.Load(Content);
            //ERROR LOADING THE SONG ?!?! HERE
           // this.song = Content.Load<Song>("theme1");
            MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;
            //MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

                       }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // We update only the currently active menu (or the running game) using the GameMenuManager
            if (GameMenuManager.mainMenuOn)
            {
                //update the main menu
                startGameScreen.Update(gameTime, this);
            }
            else if (GameMenuManager.endGameMenuOn)
            {
                // update teh end game screen
                endGameScreen.Update(gameTime, this);
            }
            else //here it should be "if (gameOn)" //TODO link all the game activity together
            {
                mainPlayer.Update(gameTime);
                foreach (var enemy in enemiesList)
                {
                    enemy.Update(gameTime, mainPlayer.getPosition());
                }

                enemy1.Update();


                if (Keyboard.GetState().IsKeyDown(Keys.F))
                    mainPlayer.playerHP -= 3;
                if (Keyboard.GetState().IsKeyDown(Keys.G))
                    mainPlayer.playerHP += 3;

                foreach (CollisionTiles tile in map.CollisionTiles)
                //  VERY HIGH Performance hit. 4096 checks for every entity in the game
                {
                    mainPlayer.Collision(tile.Rectangle);

                    foreach (var enemy in enemiesList) //
                        enemy.Collision(tile.Rectangle, map.Widht, map.Height);

                    camera.Update(mainPlayer.getPosition(), map.Widht, map.Height);
                gameUI.Update(mainPlayer.playerHP, Camera.centre);
                }


                //update the SCORES in the scoreboard AFTER the player dies or clears the level
                //first we need a Score object containing the player name and scores
                //  Score playerScore = new Score(name, points);
                //  endGameScreen.UpdateScoreboard(playerScore);
            }
            base.Update(gameTime);

        }

        private void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.4f;
            MediaPlayer.Play(song);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //the same as the update, here we draw only the active menu
            if (GameMenuManager.mainMenuOn)
            {
                startGameScreen.Draw(spriteBatch);
            }
            else if (GameMenuManager.endGameMenuOn)
            {
                endGameScreen.Draw(spriteBatch);
            }
            else //NOTE THAT WE NEED A SEPARATE spriteBatch.Begin()/End() for each menu- the menus dont work with the line below
            {
                spriteBatch.Begin(SpriteSortMode.Deferred,
                                 BlendState.AlphaBlend, null, null, null, null, camera.Transform);

                spriteBatch.Draw(backText, backRect, Color.White);

                map.Draw(spriteBatch);
                mainPlayer.Draw(spriteBatch);
                //enemy1.Draw(spriteBatch, new Vector2(330, 210));
                gameUI.Draw(spriteBatch);

                foreach (var enemy in enemiesList)
                    enemy.Draw(spriteBatch);
                spriteBatch.Draw(backText, backRect, Color.White);

                map.Draw(spriteBatch);
                mainPlayer.Draw(spriteBatch);
                //enemy1.Draw(spriteBatch, new Vector2(330, 210));
                gameUI.Draw(spriteBatch);

                foreach (var enemy in enemiesList)
                {
                    enemy.Draw(spriteBatch);
                }
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
