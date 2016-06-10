using System.Collections.Generic;
using DaGeim.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DaGeim
{
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private EndGameScreen endGameScreen;
        private HUD gameUI;
        Texture2D backText;
        Rectangle backRect;
        private Vector2 playerPosition;


        Camera camera;
        Map map;
        Player player;
        PlayerNew mainPlayer;

        private Enemy1 enemy1;
        private EnemyGuardian enemy2;
        private EnemyGuardian enemy3;
        private List<EnemyGuardian> enemiesList = new List<EnemyGuardian>();
        private Texture2D enemy1Texture2D;

        public MainGame()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            //      THE ENDGAMESCREEN IS SET FOR 1280x720
            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;
            //graphics.ApplyChanges();
        }

        public Vector2 PlayerPosition
        {
            get { return this.playerPosition; }
        }

        protected override void Initialize()
        {
            map = new Map();
            player = new Player();
            mainPlayer = new PlayerNew(new Vector2(64, 355));
            gameUI = new HUD();

            enemy2 = new EnemyGuardian();
            enemy2.StartPoint = new Vector2(164,380);
            enemy2.Position = enemy2.StartPoint;
            enemy3 = new EnemyGuardian();
            enemy3.StartPoint = new Vector2(300, 320);
            enemy3.Position = enemy3.StartPoint;
            endGameScreen = new EndGameScreen();
            base.Initialize();
        }


        protected override void LoadContent()
        {
            DrawRect.LoadContent(Content);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backText = Content.Load<Texture2D>("background");
            gameUI.Load(Content);
            backRect = new Rectangle(0, -50, 3000, 500);
            Tiles.Content = Content;
            camera = new Camera(GraphicsDevice.Viewport);
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
            player.Load(Content);
            mainPlayer.LoadContent(Content);

            this.enemiesList.Add(this.enemy2);
            this.enemiesList.Add(this.enemy3);

            foreach (var enemy in enemiesList)
            {
                enemy.Load(Content);
            }

            Texture2D enemyTexture2D = Content.Load<Texture2D>("enemy1");
            enemy1 = new Enemy1(enemyTexture2D, 2, 4);
            //loading the scoreboard content
            endGameScreen.Load(Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            mainPlayer.Update(gameTime);
            player.Update(gameTime);

            foreach (var enemy in enemiesList)
            {
                //enemy.Update(gameTime, player.Position);
                enemy.Update(gameTime, mainPlayer.getPosition());
            }
            
            enemy1.Update();

            gameUI.Update(mainPlayer.playerHP);

            if (Keyboard.GetState().IsKeyDown(Keys.F))
                mainPlayer.playerHP -= 3;
            if (Keyboard.GetState().IsKeyDown(Keys.G))
                mainPlayer.playerHP += 3;

            foreach (CollisionTiles tile in map.CollisionTiles)
            {
                mainPlayer.Collision(tile.Rectangle);
                player.Collision(tile.Rectangle, map.Widht, map.Height);

                foreach (var enemy in enemiesList)
                {
                    enemy.Collision(tile.Rectangle, map.Widht, map.Height);
                }

                camera.Update(mainPlayer.getPosition(), map.Widht, map.Height);
            }

            //update the scoreboard (the whole scoreboard screen)
            // endGameScreen.Update(gameTime, this);

            //update the SCORES in the scoreboard AFTER the player dies or clears the level
            //first we need a Score object containing the player name and scores
            //  Score playerScore = new Score(name, points);
            //  endGameScreen.UpdateScoreboard(playerScore);
            base.Update(gameTime);
        }

        private void EnemyMovement()
        {
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred,
                               BlendState.AlphaBlend, null, null, null, null, camera.Transform);

            spriteBatch.Draw(backText, backRect, Color.White);
            map.Draw(spriteBatch);
            mainPlayer.Draw(spriteBatch);
            //enemy1.Draw(spriteBatch, new Vector2(330, 210));
            gameUI.Draw(spriteBatch);

            foreach (var enemy in enemiesList)
            {
                enemy.Draw(spriteBatch);
            }
            
            //draw the scoreboard screen (after the game ends and after the Scores are updated)
            // endGameScreen.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
