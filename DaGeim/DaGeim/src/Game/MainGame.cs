namespace DaGeim.Game
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using DaGeim.Collectables;
    using DaGeim.Entities;
    using DaGeim.Entities.Bosses;
    using DaGeim.Entities.Enemies;
    using DaGeim.Entities.Player;
    using DaGeim.Helper_Classes;
    using DaGeim.Interfaces;
    using DaGeim.Level;
    using DaGeim.MenuLayouts;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using Game = Microsoft.Xna.Framework.Game;

    public class MainGame : Game
    {
        public const int WindowWidth = 1280;
        public const int WindowHeight = 720;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Song song;
        private StartGameScreen startGameScreen;
        private EndGameScreen endGameScreen;
        private PauseGameScreen pauseGameScreen;
        private CreditsScreen creditsScreen;
        private HUD gameUI;
        private Camera camera;
        private Map map;
        private Player player;
        private Boss_L1 bossL1;
        private List<NPC> npcs;
        private List<ICollectable> collectableItems;
        private List<int> deadEnemies;

        public MainGame()
        {
            this.Content.RootDirectory = "Content";
            this.Window.Title = "Robot Boy";
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = WindowWidth;
            this.graphics.PreferredBackBufferHeight = WindowHeight;
            this.graphics.ApplyChanges();
        }

        protected override void Initialize()
        {

            this.startGameScreen = new StartGameScreen();
            this.endGameScreen = new EndGameScreen();
            this.pauseGameScreen = new PauseGameScreen();
            this.creditsScreen = new CreditsScreen();
            //GameMenuManager.mainMenuOn = true;
            this.map = new Map();
            this.gameUI = new HUD();
            this.player = new Player(new Vector2(150, 465));
            this.npcs = new List<NPC>();
            this.collectableItems = new List<ICollectable>();
            this.deadEnemies = new List<int>();
            this.InitializeEnemies();
            this.InitializeCollectables();
            base.Initialize();
        }


        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.startGameScreen.Load(this.Content);
            this.pauseGameScreen.Load(this.Content);
            this.creditsScreen.Load(this.Content);
            this.endGameScreen.Load(this.Content);
            this.camera = new Camera(this.GraphicsDevice.Viewport);

            Tiles.Content = this.Content;
            this.map.Load(this.map, this.Content);
            this.player.LoadContent(this.Content);

            foreach (var npc in this.npcs)
            {
                npc.LoadContent(Content);
            }

            foreach (var collectable in this.collectableItems)
            {
                collectable.Load(Content);
            }

            DrawRect.LoadContent(this.Content);
            this.gameUI.Load(this.Content);

            this.song = this.Content.Load<Song>("theme1");
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
                this.startGameScreen.Update(gameTime, this);
            }
            else if (GameMenuManager.endGameMenuOn)
            {
                IsMouseVisible = true;
                // update teh end game screen
                this.endGameScreen.Update(gameTime, this);
            }
            else if (GameMenuManager.creditsMenuOn)
            {
                IsMouseVisible = true;
                //update the credits screen
                this.creditsScreen.Update(gameTime, this);
            }
            else if (GameMenuManager.pauseMenuOn)
            {
                IsMouseVisible = true;
                //update the pause game screen
                this.pauseGameScreen.Update(gameTime, this);
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

                this.deadEnemies.Clear();
                this.map.Update(this.player.Position);
                this.player.Update(gameTime);

                if (this.bossL1.isPushing)
                    this.player.PushedByBoss();

                foreach (var npc in this.npcs)
                {
                    npc.Update(gameTime);
                    npc.Shoot(this.player);
                }

                int tempScore = 0;
                for (int i = 0; i < this.npcs.Count; i++)
                {
                    if (this.npcs[i].Dead)
                    {
                        this.deadEnemies.Add(i);
                        tempScore += 50;
                    }
                }
                this.player.Score = tempScore;
                foreach (var index in this.deadEnemies)
                    this.npcs.RemoveAt(index);

                foreach (ICollectable collectable in this.collectableItems)
                    collectable.Update(gameTime);

                this.MakeCollisionWithMap();
                this.CollisionWithRocket();
                this.CollisionWithEnemy();
                this.CollisionWithAmmo();

                this.camera.Update(this.player.Position, this.map.Widht, this.map.Height);
                this.gameUI.Update(this.player.Health, Camera.centre);

                if (this.player.Health <= 0)
                {
                    this.Initialize();
                    this.endGameScreen.UpdateScoreboard(player.Score);
                    Thread.Sleep(100);
                    GameMenuManager.endGameMenuOn = true;
                    GameMenuManager.gameOn = false;
                    GameMenuManager.TurnOtherMenusOff();
                }
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //the same as the update, here we draw only the active menu
            if (GameMenuManager.mainMenuOn)
            {
                this.startGameScreen.Draw(this.spriteBatch);
            }
            else if (GameMenuManager.endGameMenuOn)
            {
                this.endGameScreen.Draw(this.spriteBatch);
            }
            else if (GameMenuManager.pauseMenuOn)
            {
                this.pauseGameScreen.Draw(this.spriteBatch);
            }
            else if (GameMenuManager.creditsMenuOn)
            {
                this.creditsScreen.Draw(this.spriteBatch);
            }
            else
            {
                this.spriteBatch.Begin(SpriteSortMode.Deferred,
                                 BlendState.AlphaBlend, null, null, null, null, this.camera.Transform);

                this.map.Draw(this.spriteBatch);
                this.player.Draw(this.spriteBatch);

                this.gameUI.Draw(this.spriteBatch, this.player);
                foreach (var rocket in this.player.Rockets)
                    rocket.Draw(this.spriteBatch);

                foreach (var enemy in this.npcs)
                    enemy.Draw(this.spriteBatch);

                foreach (var collectable in collectableItems)
                    collectable.Draw(spriteBatch);

                this.spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        private void CollisionWithAmmo()
        {
            foreach (var npc in this.npcs)
            {
                foreach (var ammonition in npc.ammo)
                {
                    if (ammonition.CollisionBox.Intersects(this.player.CollisionBox))
                    {
                        ammonition.IsVisible = false;
                        this.player.Health -= 20;
                    }
                }
            }
        }
        private void CollisionWithEnemy()
        {
            //enemy collision with enemy
            for (int i = 0; i < this.npcs.Count - 1; i++)
            {
                for (int j = i + 1; j < this.npcs.Count - 1; j++)
                {
                    this.npcs[i].CollisionWithEntity(this.npcs[j]);
                }
            }

            // Player collision with collectables
            foreach (var collectable in this.collectableItems)
            {
                this.player.CollisionWithCollectable(collectable); //TODO: Add CollisionWithCollectable
                collectable.CollisionWithPlayer(this.player);
            }

        }


        private void CollisionWithRocket()
        {
            // player rockets collision with enemies
            foreach (var rocket in this.player.Rockets)
            {
                foreach (var npc in this.npcs)
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

            foreach (var npc in this.npcs)
            {
                startTileIndex = this.CalculateStartTileIndex(npc.Position);
                endTileIndex = Math.Min(this.map.CollisionTiles.Count - 1, startTileIndex + 40);

                for (int i = startTileIndex; i <= endTileIndex; i++)
                {
                    npc.CollisionWithMap(this.map.CollisionTiles[i].Rectangle, this.map.Widht, this.map.Height);
                }
            }

            // player's rockets collision with map
            foreach (var rocket in this.player.Rockets)
            {
                startTileIndex = this.CalculateStartTileIndex(rocket.Position);
                endTileIndex = Math.Min(this.map.CollisionTiles.Count - 1, startTileIndex + 40);

                for (int i = startTileIndex; i <= endTileIndex; i++)
                {
                    rocket.CollisionWithMap(this.map.CollisionTiles[i].Rectangle, this.map.Widht, this.map.Height);
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
            this.bossL1 = new Boss_L1(new Vector2(5250, 450), 100);

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

            this.npcs.Add(enemy1);
            this.npcs.Add(enemy2);
            this.npcs.Add(enemy4);
            this.npcs.Add(enemy5);
            // npcs.Add(enemy6);
            this.npcs.Add(enemy7);
            this.npcs.Add(enemy8);
            this.npcs.Add(enemy9);
            // npcs.Add(enemy10);
            // npcs.Add(enemy11);
            this.npcs.Add(enemy12);
            this.npcs.Add(enemy13);
            this.npcs.Add(enemy14);
            this.npcs.Add(bossL1);
        }

        public void InitializeCollectables()
        {
            HealthRestore healthRestore1 = new HealthRestore(new Vector2(700, 600));
            HealthRestore healthRestore2 = new HealthRestore(new Vector2(800, 225));
            JumpBooster jumpBooster2 = new JumpBooster(new Vector2(2800, 150));
            JumpBooster jumpBooster1 = new JumpBooster(new Vector2(1000, 250));
            HealthRestore healthRestore5 = new HealthRestore(new Vector2(5000, 400));
            HealthRestoreBig healthRestore6 = new HealthRestoreBig(new Vector2(4400, 100));
            Chest chest1 = new Chest(new Vector2(5450, 518));
            RocketShootingBooster rocketShootingBooster = new RocketShootingBooster(new Vector2(750, 225));
            this.collectableItems.Add(healthRestore1);
            this.collectableItems.Add(healthRestore2);
            this.collectableItems.Add(jumpBooster2);
            this.collectableItems.Add(jumpBooster1);
            this.collectableItems.Add(healthRestore5);
            this.collectableItems.Add(healthRestore6);
            this.collectableItems.Add(chest1);
            this.collectableItems.Add(rocketShootingBooster);
        }
        //private void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        //{
        //    // 0.0f is silent, 1.0f is full volume
        //    MediaPlayer.Volume -= 0.1f;
        //    MediaPlayer.Play(song);
        //}
    }
}
