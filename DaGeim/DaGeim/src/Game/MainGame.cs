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
        private ScoreBoard scoreBoard;

        Texture2D backText;
        Rectangle backRect;


        Camera camera;
        Map map;
        Player player;
        PlayerNew mainPlayer;

        private Enemy1 enemy1;
        private Enemy2 enemy2;
        private Enemy2 enemy3;
        private Texture2D enemy1Texture2D;

        public MainGame()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            //      THE SCOREBOARD IS SET FOR 1280x720
            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;
            //graphics.ApplyChanges();
        }


        protected override void Initialize()
        {
            map = new Map();
            player = new Player();
            mainPlayer = new PlayerNew(new Vector2(64, 355));

            enemy2 = new Enemy2();
            enemy2.Position = new Vector2(164, 380);
            enemy3 = new Enemy2();
            enemy3.Position = new Vector2(330, 320);
            scoreBoard = new ScoreBoard();
            base.Initialize();
        }


        protected override void LoadContent()
        {
            DrawRect.LoadContent(Content);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backText = Content.Load<Texture2D>("background");
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
            enemy2.Load(Content);
            enemy3.Load(Content);

            mainPlayer.LoadContent(Content);


            Texture2D enemyTexture2D = Content.Load<Texture2D>("enemy1");
            enemy1 = new Enemy1(enemyTexture2D, 2, 4);
            //loading the scoreboard content
            // scoreBoard.Load(Content);
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
            enemy2.Update(gameTime);
            enemy3.Update(gameTime);
            enemy1.Update();

            foreach (CollisionTiles tile in map.CollisionTiles)
            {
                mainPlayer.Collision(tile.Rectangle);
                player.Collision(tile.Rectangle, map.Widht, map.Height);
                enemy2.Collision(tile.Rectangle, map.Widht, map.Height);
                enemy3.Collision(tile.Rectangle, map.Widht, map.Height);

                camera.Update(mainPlayer.getPosition(), map.Widht, map.Height);
            }
            //update the scoreboard (the whole scoreboard screen)
            //scoreBoard.Update(gameTime, this);

            //update the SCORES in the scoreboard AFTER the player dies or clears the level
            //first we need a Score object containing the player name and scores
            //  Score playerScore = new Score(name, points);
            //  scoreBoard.UpdateScore(playerScore);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred,
                               BlendState.AlphaBlend, null, null, null, null, camera.Transform);
            spriteBatch.Draw(backText, backRect, Color.White);
            map.Draw(spriteBatch);
            mainPlayer.Draw(spriteBatch);
            enemy1.Draw(spriteBatch, new Vector2(330, 210));
//            enemy2.Draw(spriteBatch);
//            enemy3.Draw(spriteBatch);
            //draw the scoreboard screen (after the game ends and after the Scores are updated)
            //scoreBoard.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
