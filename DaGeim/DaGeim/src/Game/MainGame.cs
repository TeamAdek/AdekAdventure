using System.Collections.Generic;
using System.Threading;
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
        public const int GAME_WIDTH = 1280;
        public const int GAME_HEIGHT = 720;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Song song;
        private StartGameScreen startGameScreen;
        private EndGameScreen endGameScreen;
        private PauseGameScreen pauseGameScreen;
        private CreditsScreen creditsScreen;
        private HUD gameUI;
        Texture2D backText;
        Rectangle backRect;

        Camera camera;
        Map map;

        Player mainPlayer;
        Skeleton skeletonEnemy;
        private List<IEntity> entities;
        private List<Skeleton> enemies = new List<Skeleton>();
        private List<int> deadEnemies = new List<int>();


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
            startGameScreen = new StartGameScreen();
            endGameScreen = new EndGameScreen();
            pauseGameScreen = new PauseGameScreen();
            creditsScreen = new CreditsScreen();
            
            map = new Map();
            gameUI = new HUD();

            mainPlayer = new Player(new Vector2(100, 350));
            skeletonEnemy = new Skeleton(new Vector2(700, 520), 120);
            InitializeEnemies();

            entities = new List<IEntity>();
            base.Initialize();
        }



        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            startGameScreen.Load(Content);
            pauseGameScreen.Load(Content);
            creditsScreen.Load(Content);
            endGameScreen.Load(Content);

            camera = new Camera(GraphicsDevice.Viewport);

            Tiles.Content = Content;
            map.Load(map);

            mainPlayer.LoadContent(Content);

            foreach (var enemy in enemies)
            {
                enemy.Load(Content);
            }
            skeletonEnemy.Load(Content);


            DrawRect.LoadContent(Content);
            backText = Content.Load<Texture2D>("background");
            backRect = new Rectangle(0, -50, 6000, 700);
            gameUI.Load(Content);

           // song = Content.Load<Song>("theme1");
            MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;


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
            else if (GameMenuManager.creditsMenuOn)
            {
                //update the credits screen
                creditsScreen.Update(gameTime, this);
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

                for (int i = 0; i < enemies.Count; i++)
                {
                    if(enemies[i].dead)
                        deadEnemies.Add(i);
                }
                foreach (var index in deadEnemies)
                    enemies.RemoveAt(index);
                deadEnemies.Clear();
                
                mainPlayer.Update(gameTime);

                skeletonEnemy.Update(gameTime, mainPlayer.getPosition());
                foreach (var enemy in enemies)
                    enemy.Update(gameTime, mainPlayer.getPosition());

                MakeCollisionWithMap();
                CollisionWithRocket();
                CollisionWithEnemy();


                //TODO add collision between player and enemies
                //TODO add collision between player/enemy and rockets

                camera.Update(mainPlayer.getPosition(), map.Widht, map.Height);
                gameUI.Update(mainPlayer.playerHP, Camera.centre);
                //  }
                /*---------------------------------------------------------------
                if the player dies we update the scoreboard
                ---------------------------------------------------------------*/
                if (mainPlayer.playerHP <= 0)
                {
                    endGameScreen.UpdateScoreboard(mainPlayer.playerScore);
                    Thread.Sleep(100);
                    GameMenuManager.endGameMenuOn = true;
                    GameMenuManager.gameOn = false;
                    GameMenuManager.TurnOtherMenusOff();
                }
            }
            base.Update(gameTime);
        }

        private void CollisionWithEnemy()
        {
            // Player collision with enemy
            foreach (var enemy in enemies)
            {
                mainPlayer.CollisionWithEntity(enemy);   
            }
            mainPlayer.CollisionWithEntity(skeletonEnemy);
            //enemy collision with enemy
            for (int i = 0; i < enemies.Count - 1; i++)
            {
                for (int j = i + 1; j < enemies.Count - 1; j++)
                {
                    enemies[i].CollisionWithEntity(enemies[j]);
                    skeletonEnemy.CollisionWithEntity(enemies[j]);
                }
            }
            
        }

        private void CollisionWithRocket()
        {
            // player rockets collision with enemies
            foreach (var rocket in this.mainPlayer.Rockets)
            {
                foreach (var enemy in enemies)
                {
                    enemy.CollisionWithRocket(rocket, mainPlayer);
                }
            }

            // Enemy rockets collision with player
//            foreach (var enemy in this.enemies)
//            {
//                if (enemy.Rockets != null)
//                {
//                    foreach (var rocket in enemy.Rockets)
//                    {
//                        this.mainPlayer.CollisionWithRocket(rocket);
//                    }
//                }
//
//            }

            // Collision rocket with rocket if needed
        }


        private void MakeCollisionWithMap()
        {
            // player collison with map
            int startTileIndex = this.CalculateStartTileIndex(new Vector2(this.mainPlayer.collisionBox.X, this.mainPlayer.collisionBox.Y));
            int endTileIndex = Math.Min(this.map.CollisionTiles.Count - 1, startTileIndex + 40);

            for (int i = startTileIndex; i <= endTileIndex; i++)
            {
                this.mainPlayer.CollisionWithMap(this.map.CollisionTiles[i].Rectangle, this.map.Widht, this.map.Height);
                   skeletonEnemy.CollisionWithMap(this.map.CollisionTiles[i].Rectangle, map.Widht, this.map.Height);

            }

            foreach (var enemy in enemies)
            {
                startTileIndex = this.CalculateStartTileIndex(enemy.Position);
                endTileIndex = Math.Min(this.map.CollisionTiles.Count - 1, startTileIndex + 40);

                for (int i = startTileIndex; i <= endTileIndex; i++)
                {
                    enemy.CollisionWithMap(this.map.CollisionTiles[i].Rectangle, map.Widht, this.map.Height);
                }  
            }

            // player's rockets collision with map
            foreach (var rocket in this.mainPlayer.Rockets)
            {
                startTileIndex = this.CalculateStartTileIndex(rocket.shootPosition);
                endTileIndex = Math.Min(this.map.CollisionTiles.Count - 1, startTileIndex + 40);

                for (int i = startTileIndex; i <= endTileIndex; i++)
                {
                    rocket.Collision(this.map.CollisionTiles[i].Rectangle);
                }
            }

            // enemies rockets collision with map
//            foreach (var enemy in this.enemiesList)
//            {
//                if (enemy.Rockets != null)
//                {
//                    foreach (var rocket in enemy.Rockets)
//                    {
//                        startTileIndex = this.CalculateStartTileIndex(rocket.shootPosition);
//                        endTileIndex = Math.Min(this.map.CollisionTiles.Count - 1, startTileIndex + 40);
//
//                        for (int i = startTileIndex; i <= endTileIndex; i++)
//                        {
//                            rocket.Collision(this.map.CollisionTiles[i].Rectangle);
//                        }
//                    }
//                }
//            }
        }

        private int CalculateStartTileIndex(Vector2 entityPosition)
        {
            int xPosition = Math.Max(0, (int)(entityPosition.X / this.map.TileSize) * this.map.TileSize - this.map.TileSize);
            Rectangle startTileRectangle = new Rectangle(xPosition, 0, 0, 0);
            int startTileIndex = Math.Max(0, this.map.CollisionTiles.BinarySearch(new CollisionTiles(1, startTileRectangle)) - 20);

            return startTileIndex;
        }

        private void InitializeEnemies()
        {
            Skeleton enemy1 = new Skeleton(new Vector2(910, 350), 150);
            Skeleton enemy2 = new Skeleton(new Vector2(1200, 500), 150);
            Skeleton enemy3 = new Skeleton(new Vector2(1400, 200), 200);
            Skeleton enemy4 = new Skeleton(new Vector2(740, 100), 90);
            Skeleton enemy5 = new Skeleton(new Vector2(1800, 100), 200);
            Skeleton enemy6 = new Skeleton(new Vector2(2200, 155), 50);
            Skeleton enemy7 = new Skeleton(new Vector2(1950, 155), 40);
            Skeleton enemy8 = new Skeleton(new Vector2(2400, 200), 75);
            Skeleton enemy9 = new Skeleton(new Vector2(2600, 155), 150);
            Skeleton enemy10 = new Skeleton(new Vector2(2800, 155), 100);
            Skeleton enemy11 = new Skeleton(new Vector2(3000, 320), 100);
            Skeleton enemy12 = new Skeleton(new Vector2(3800, 340), 150);
            Skeleton enemy13 = new Skeleton(new Vector2(4100, 190), 25);
            Skeleton enemy14 = new Skeleton(new Vector2(4300, 190), 75);
            Skeleton enemy15 = new Skeleton(new Vector2(4600, 200), 75);
            Skeleton enemy16 = new Skeleton(new Vector2(4700, 200), 45);
            Skeleton enemy17 = new Skeleton(new Vector2(4800, 200), 65);
            Skeleton enemy18 = new Skeleton(new Vector2(4900, 200), 105);
            Skeleton enemy19 = new Skeleton(new Vector2(5000, 200), 35);
            Skeleton enemy20 = new Skeleton(new Vector2(5200, 200), 95);


            enemies.Add(enemy1);
            enemies.Add(enemy2);
            enemies.Add(enemy3);
            enemies.Add(enemy4);
            enemies.Add(enemy5);
            enemies.Add(enemy6);
            enemies.Add(enemy7);
            enemies.Add(enemy8);
            enemies.Add(enemy9);
            enemies.Add(enemy10);
            enemies.Add(enemy11);
            enemies.Add(enemy12);
            enemies.Add(enemy13);
            enemies.Add(enemy14);
            enemies.Add(enemy15);
            enemies.Add(enemy16);
            enemies.Add(enemy17);
            enemies.Add(enemy18);
            enemies.Add(enemy19);
            enemies.Add(enemy20);
        }

        private void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
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
            else if (GameMenuManager.creditsMenuOn)
            {
                creditsScreen.Draw(spriteBatch);
            }
            else //NOTE THAT WE NEED A SEPARATE spriteBatch.Begin()/End() for each menu- the menus dont work with the line below
            {
                spriteBatch.Begin(SpriteSortMode.Deferred,
                                 BlendState.AlphaBlend, null, null, null, null, camera.Transform);

                spriteBatch.Draw(backText, backRect, Color.White);

                map.Draw(spriteBatch);
                mainPlayer.Draw(spriteBatch);
                skeletonEnemy.Draw(spriteBatch);
                gameUI.Draw(spriteBatch);

                foreach (var enemy in enemies)
                {
                    enemy.Draw(spriteBatch);
                }

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
