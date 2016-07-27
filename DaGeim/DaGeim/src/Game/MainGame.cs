using System;
using System.Collections.Generic;
using System.Threading;
using DaGeim.Collectables;
using DaGeim.Interfaces;
using DaGeim.MenuLayouts;
using DaGeim.src.Collectable;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DaGeim.Game
{
    using Game = Microsoft.Xna.Framework.Game;

    public class MainGame : Game
    {
        public const int WindowWidth = 1280;
        public const int WindowHeight = 720;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Song song;
        private StartGameScreen startGameScreen;
        private EndGameScreen endGameScreen;
        private PauseGameScreen pauseGameScreen;
        private CreditsScreen creditsScreen;
        private HUD gameUI;


        Camera camera;
        Map map;

        private Player player;
        private Boss_L1 bossL1;
        private List<NPC> npcs = new List<NPC>();
        private List<ICollectable> collectableItems = new List<ICollectable>();
        private List<int> deadEnemies = new List<int>();

        public MainGame()
        {
            Content.RootDirectory = "Content";
            Window.Title = "Robot Boy";
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            GameMenuManager.mainMenuOn = true;
            startGameScreen = new StartGameScreen();
            endGameScreen = new EndGameScreen();
            pauseGameScreen = new PauseGameScreen();
            creditsScreen = new CreditsScreen();

            GameMenuManager.mainMenuOn = true;
            map = new Map();
            gameUI = new HUD();

            
            player = new Player(new Vector2(150, 465));

            InitializeEnemies();
            InitializeCollectables();
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
            map.Load(map, Content);
            player.LoadContent(Content);

            foreach (var npc in npcs)
            {
                npc.LoadContent(Content);
            }
            
            foreach (var collectable in collectableItems)
            {
                collectable.Load(Content);
            }

            DrawRect.LoadContent(Content);
            gameUI.Load(Content);

            song = Content.Load<Song>("theme1");
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
                IsMouseVisible = true;
                startGameScreen.Update(gameTime, this);
            }
            else if (GameMenuManager.endGameMenuOn)
            {
                IsMouseVisible = true;
                // update teh end game screen
                endGameScreen.Update(gameTime, this);
            }
            else if (GameMenuManager.creditsMenuOn)
            {
                IsMouseVisible = true;
                //update the credits screen
                creditsScreen.Update(gameTime, this);
            }
            else if (GameMenuManager.pauseMenuOn)
            {
                IsMouseVisible = true;
                //update the pause game screen
                pauseGameScreen.Update(gameTime, this);
            }
            else  
            {
                IsMouseVisible = false;
                //pressing esc we call the pause game screen
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    GameMenuManager.pauseMenuOn = true;
                    GameMenuManager.gameOn = false;
                    GameMenuManager.TurnOtherMenusOff();
                }

                deadEnemies.Clear();
                map.Update(player.Position);
                player.Update(gameTime);

                if(bossL1.isPushing)
                    player.PushedByBoss();

                foreach (var npc in npcs)
                {
                    npc.Update(gameTime);
                    npc.Shoot(player);
                }

		int tempScore = 0;
                for (int i = 0; i < npcs.Count; i++)
                {
                    if (npcs[i].Dead)
                    {
                        deadEnemies.Add(i);
                 	tempScore += 50;       
                    }
                }
                player.Score = tempScore;
                foreach (var index in deadEnemies)
                    npcs.RemoveAt(index);

                foreach (ICollectable collectable in collectableItems)
                    collectable.Update(gameTime);

                MakeCollisionWithMap();
                CollisionWithRocket();
                CollisionWithEnemy();
                CollisionWithAmmo();

                camera.Update(player.Position, map.Widht, map.Height);
                gameUI.Update(player.Health, Camera.centre);

                if (player.Health <= 0)
                {
                    endGameScreen.UpdateScoreboard(player.Score);
                    Thread.Sleep(100);
                    GameMenuManager.endGameMenuOn = true;
                    GameMenuManager.gameOn = false;
                    GameMenuManager.TurnOtherMenusOff();
                }
            }

            base.Update(gameTime);
        }

        private void CollisionWithAmmo()
        {
            foreach (var npc in npcs)
            {
                foreach (var ammonition in npc.ammo)
                {
                    if (ammonition.CollisionBox.Intersects(player.CollisionBox))
                    {
                        ammonition.IsVisible = false;
                        player.Health -= 20;
                    }
                }
            }
        }
        private void CollisionWithEnemy()
        {
            //enemy collision with enemy
            for (int i = 0; i < npcs.Count - 1; i++)
            {
                for (int j = i + 1; j < npcs.Count - 1; j++)
                {
                    npcs[i].CollisionWithEntity(npcs[j]);
                }
            }

            // Player collision with collectables
            foreach (var collectable in collectableItems)
            {
                player.CollisionWithCollectable(collectable); //TODO: Add CollisionWithCollectable
                collectable.CollisionWithPlayer(player);
            }

        }


        private void CollisionWithRocket()
        {
            // player rockets collision with enemies
            foreach (var rocket in player.Rockets)
            {
                foreach (var npc in npcs)
                {
                    npc.CollisionWithAmmunition(rocket);
                }
            }
        }

        private void MakeCollisionWithMap()
        {
            // player collison with map
            int startTileIndex = this.CalculateStartTileIndex(new Vector2(this.player.CollisionBox.X, this.player.CollisionBox.Y));
            int endTileIndex = Math.Min(this.map.CollisionTiles.Count - 1, startTileIndex + 40);

            for (int i = startTileIndex; i <= endTileIndex; i++)
            {
                this.player.CollisionWithMap(this.map.CollisionTiles[i].Rectangle, this.map.Widht, this.map.Height);
            }

            foreach (var npc in npcs)
            {
                startTileIndex = this.CalculateStartTileIndex(npc.Position);
                endTileIndex = Math.Min(this.map.CollisionTiles.Count - 1, startTileIndex + 40);

                for ( int i = startTileIndex; i <= endTileIndex; i++)
                {
                    npc.CollisionWithMap(this.map.CollisionTiles[i].Rectangle, map.Widht, this.map.Height);
                }
            }

            // player's rockets collision with map
            foreach (var rocket in this.player.Rockets)
            {
                startTileIndex = this.CalculateStartTileIndex(rocket.Position);
                endTileIndex = Math.Min(this.map.CollisionTiles.Count - 1, startTileIndex + 40);

                for (int i = startTileIndex; i <= endTileIndex; i++)
                {
                    rocket.CollisionWithMap(this.map.CollisionTiles[i].Rectangle, map.Widht, this.map.Height);
                }
            }
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
            Skeleton enemy1 = new Skeleton(new Vector2(910, 450), 350);
            bossL1 = new Boss_L1(new Vector2(5250, 450), 100);

            Skeleton enemy2 = new Skeleton(new Vector2(1100, 500), 50);
            Skeleton enemy4 = new Skeleton(new Vector2(710, 200), 70);
            Skeleton enemy5 = new Skeleton(new Vector2(1800, 100), 200);
          //  Skeleton enemy6 = new Skeleton(new Vector2(2200, 155), 50);
            Skeleton enemy7 = new Skeleton(new Vector2(1950, 155), 40);
            Skeleton enemy8 = new Skeleton(new Vector2(2400, 200), 75);
            Skeleton enemy9 = new Skeleton(new Vector2(2600, 155), 150);
           // Skeleton enemy10 = new Skeleton(new Vector2(2800, 155), 100);
            //Skeleton enemy11 = new Skeleton(new Vector2(3000, 320), 100);
            Skeleton enemy12 = new Skeleton(new Vector2(3800, 340), 150);
            Skeleton enemy13 = new Skeleton(new Vector2(4100, 190), 25);
            Skeleton enemy14 = new Skeleton(new Vector2(4300, 190), 75);

            npcs.Add(enemy1);
            npcs.Add(enemy2);
            npcs.Add(enemy4);
            npcs.Add(enemy5);
           // npcs.Add(enemy6);
            npcs.Add(enemy7);
            npcs.Add(enemy8);
            npcs.Add(enemy9);
           // npcs.Add(enemy10);
           // npcs.Add(enemy11);
            npcs.Add(enemy12);
            npcs.Add(enemy13);
            npcs.Add(enemy14);
            npcs.Add(bossL1);
        }

        public void InitializeCollectables()
        {
            HealthRestore healthRestore1 = new HealthRestore(new Vector2(700, 600));
            HealthRestore healthRestore2 = new HealthRestore(new Vector2(800, 230));
            JumpBooster jumpBooster2 = new JumpBooster(new Vector2(2800, 150));
            JumpBooster jumpBooster1 = new JumpBooster(new Vector2(1000, 250));
            HealthRestore healthRestore5 = new HealthRestore(new Vector2(5000, 400));
            HealthRestoreBig healthRestore6 = new HealthRestoreBig(new Vector2(4400, 100));
            Chest chest1 = new Chest(new Vector2(5450, 518));
            collectableItems.Add(healthRestore1);
            collectableItems.Add(healthRestore2);
            collectableItems.Add(jumpBooster2);
            collectableItems.Add(jumpBooster1);
            collectableItems.Add(healthRestore5);
            collectableItems.Add(healthRestore6);
            collectableItems.Add(chest1);
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
            else 
            {
                spriteBatch.Begin(SpriteSortMode.Deferred,
                                 BlendState.AlphaBlend, null, null, null, null, camera.Transform);

                map.Draw(spriteBatch);
                player.Draw(spriteBatch);

                gameUI.Draw(spriteBatch, player);
                foreach (var rocket in player.Rockets)
                    rocket.Draw(spriteBatch);

                foreach (var enemy in npcs)
                      enemy.Draw(spriteBatch);

                foreach (var collectable in collectableItems)
                    collectable.Draw(spriteBatch);

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
