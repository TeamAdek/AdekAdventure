using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DaGeim.Enemies;

namespace DaGeim
{
    using System;

    using Game.src.Entities;

    using Game = Microsoft.Xna.Framework.Game;

    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Song song;
        private StartGameScreen startGameScreen;
        private EndGameScreen endGameScreen;
        private PauseGameScreen pauseGameScreen;
        private double TimeForUpdate;
        private HUD gameUI;
        Texture2D backText;
        Rectangle backRect;

        Camera camera;
        Map map;

        Player mainPlayer;

        private List<IEntity> entities;
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
            GameMenuManager.mainMenuOn = true;
            // we set the mainmenuON, because we want to start from the mainMenu
            startGameScreen = new StartGameScreen();
            endGameScreen = new EndGameScreen();
            pauseGameScreen = new PauseGameScreen();
            map = new Map();
            gameUI = new HUD();

            mainPlayer = new Player(new Vector2(155, 325));

            this.entities = new List<IEntity>();
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
            pauseGameScreen.Load(Content);
            endGameScreen.Load(Content);

            camera = new Camera(GraphicsDevice.Viewport);

            Tiles.Content = Content;
            map.Load(map);


            mainPlayer.LoadContent(Content);

            Texture2D enemyTexture2D = Content.Load<Texture2D>("enemy1");
            enemy1 = new Enemy1(enemyTexture2D, 2, 4);
            this.enemiesList.Add(this.enemy2);
            this.enemiesList.Add(this.enemy3);

            foreach (var enemy in enemiesList)
                enemy.Load(Content);

            DrawRect.LoadContent(Content);
            backText = Content.Load<Texture2D>("background");
            backRect = new Rectangle(0, -50, 6000, 700);
            gameUI.Load(Content);
            //loading the endGameScreen content
            endGameScreen.Load(Content);
            //ERROR LOADING THE SONG ?!?! HERE
            //  song = Content.Load<Song>("theme1");
            //  MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;
            //MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

        }

        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {

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
            else if (GameMenuManager.pauseMenuOn)
            {
                //update the pause game screen
                pauseGameScreen.Update(gameTime, this);
            }
            else  //TODO link all the game activity together
            {
                //pressing esc we call the pause game screen
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    GameMenuManager.pauseMenuOn = true;
                    GameMenuManager.gameOn = false;
                    GameMenuManager.TurnOtherMenusOff();
                }
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

                this.MakeCollisionWithMap();

                //TODO add collision between player and enemies
                //TODO add collision between player/enemy and rockets

                //   foreach (CollisionTiles tile in map.CollisionTiles)
                //  VERY HIGH Performance hit. 4096 checks for every entity in the game
                //   {
                //    mainPlayer.CollisionWithMap(tile.Rectangle, map.Widht, map.Height);

                //     foreach (var enemy in enemiesList) //
                //         enemy.CollisionWithMap(tile.Rectangle, map.Widht, map.Height);

                camera.Update(mainPlayer.getPosition(), map.Widht, map.Height);
                gameUI.Update(mainPlayer.playerHP, Camera.centre);
                //  }
                //update the SCORES in the scoreboard AFTER the player dies or clears the level
                //first we need a Score object containing the player name and scores
                //  Score playerScore = new Score(name, points);
                //  endGameScreen.UpdateScoreboard(playerScore);
            }
            base.Update(gameTime);
        }

        private void MakeCollisionWithMap()
        {
            // player collison with map
            int startTileIndex = this.CalculateStartTileIndex(new Vector2(this.mainPlayer.collisionBox.X, this.mainPlayer.collisionBox.Y));
            int endTileIndex = Math.Min(this.map.CollisionTiles.Count - 1, startTileIndex + 24);

            for (int i = startTileIndex; i <= endTileIndex; i++)
            {
                this.mainPlayer.CollisionWithMap(this.map.CollisionTiles[i].Rectangle, this.map.Widht, this.map.Height);
            }

            //enemies collision with map
            foreach (var enemyGuardian in this.enemiesList)
            {
                startTileIndex = this.CalculateStartTileIndex(enemyGuardian.Position);
                endTileIndex = Math.Min(this.map.CollisionTiles.Count - 1, startTileIndex + 24);

                for (int i = startTileIndex; i <= endTileIndex; i++)
                {
                    enemyGuardian.CollisionWithMap(this.map.CollisionTiles[i].Rectangle, this.map.Widht, this.map.Height);
                }
            }

            // player's rockets collision with map
            foreach (var rocket in this.mainPlayer.Rockets)
            {
                startTileIndex = this.CalculateStartTileIndex(rocket.shootPosition);
                endTileIndex = Math.Min(this.map.CollisionTiles.Count - 1, startTileIndex + 30);

                for (int i = startTileIndex; i <= endTileIndex; i++)
                {
                    rocket.Collision(this.map.CollisionTiles[i].Rectangle);
                }
            }

            //TODO add enemies rocket collision with map
        }

        private int CalculateStartTileIndex(Vector2 entityPosition)
        {
            int xPosition = Math.Max(0, (int)(entityPosition.X / this.map.TileSize) * this.map.TileSize - this.map.TileSize);
            Rectangle startTileRectangle = new Rectangle(xPosition, 0, 0, 0);
            int startTileIndex = Math.Max(0, this.map.CollisionTiles.BinarySearch(new CollisionTiles(1, startTileRectangle)) - 10);

            int s = 1;
            while (startTileIndex < 0)
            {
                xPosition = Math.Max(0, (int)(entityPosition.X / this.map.TileSize) * this.map.TileSize - this.map.TileSize * s);
                startTileRectangle = new Rectangle(xPosition, 0, 0, 0);
                startTileIndex = this.map.CollisionTiles.BinarySearch(new CollisionTiles(1, startTileRectangle));
                s++;
            }

            return startTileIndex;
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
            else if (GameMenuManager.pauseMenuOn)
            {
                pauseGameScreen.Draw(spriteBatch);
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
