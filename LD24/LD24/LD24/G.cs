using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LD24
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class G : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public static G g;
        public static Random r = new Random();
        public BasicEffect e;
        public SpriteFont font;
        Screen currentScreen;
        internal List<Photograph> photos = new List<Photograph>();

        public G()
        {
            g = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Species Hunt";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
        }

        protected override void Initialize()
        {
            base.Initialize();
            RM.ConfigureKeys();
            currentScreen = new MainMenuScreen();
            currentScreen.Show();
        }

        protected override void LoadContent()
        {
            e = new BasicEffect(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            RM.font = font;
            RM.AddTexture("island", Content.Load<Texture2D>("terrain"));
            RM.AddTexture("grass", Content.Load<Texture2D>("grass"));
            RM.AddTexture("water", Content.Load<Texture2D>("water"));
            RM.AddTexture("treetrunk", Content.Load<Texture2D>("treetrunk"));
            RM.AddTexture("treeleaves", Content.Load<Texture2D>("treeleaves"));
            RM.AddTexture("player", Content.Load<Texture2D>("player"));
            RM.AddTexture("cameraoverlay", Content.Load<Texture2D>("cameraoverlay"));
            RM.AddTexture("sun", Content.Load<Texture2D>("sun"));
            RM.AddTexture("cloud1", Content.Load<Texture2D>("cloud1"));
            RM.AddTexture("cloud2", Content.Load<Texture2D>("cloud2"));
            RM.AddTexture("cloud3", Content.Load<Texture2D>("cloud3"));
            RM.AddTexture("splash1", Content.Load<Texture2D>("splash1"));
            RM.AddTexture("splash2", Content.Load<Texture2D>("splash2"));
            RM.AddTexture("splash3", Content.Load<Texture2D>("splash3"));
            RM.AddTexture("flowerstem", Content.Load<Texture2D>( "flowerstem"));
            RM.AddTexture("flowercore", Content.Load<Texture2D>( "flowercore"));
            RM.AddTexture("flowerleaves", Content.Load<Texture2D>("flowerleaves"));
            RM.AddTexture("sand", Content.Load<Texture2D>("sand"));
            RM.AddTexture("snow", Content.Load<Texture2D>("snow"));

            RM.AddSound("snap", Content.Load<SoundEffect>("snap"));
            RM.Volume = 1;
            bf = new BirdFactory();
            bf.LoadContent(Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            IM.NewState();

            currentScreen.Update();
            if (currentScreen.CanTakePhoto && RM.IsDown(InputAction.AltFire) && RM.IsPressed(InputAction.Fire))
            {
                int scale = 2;
                RenderTarget2D screenshot = new RenderTarget2D(GraphicsDevice, 800 * scale, 600 * scale, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
                GraphicsDevice.Clear(Color.Black);
                GraphicsDevice.SetRenderTarget(screenshot);
                GraphicsDevice.Clear(Color.CornflowerBlue);
                currentScreen.Draw();
                GraphicsDevice.SetRenderTarget(null);

                Color[] data = new Color[320 * 240 * scale * scale];
                screenshot.GetData<Color>(0, new Rectangle(240 * scale, 180 * scale, 320 * scale, 240 * scale), data, 0, data.Length);
                Texture2D shot = new Texture2D(GraphicsDevice, 320 * scale, 240 * scale);
                shot.SetData<Color>(data);
                Photograph pg = new Photograph(shot);
                photos.Add(pg);
                screenshot.Dispose();

                currentScreen.AddPhotoData(pg);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            currentScreen.Draw();

            base.Draw(gameTime);
        }

        public static bool HasFocus { get { return g.IsActive; } }
        public static int Width { get { return g.Window.ClientBounds.Width; } }
        public static int Height { get { return g.Window.ClientBounds.Height; } }

        internal void Showscreen(Screen newScreen)
        {
            currentScreen.Hide();
            currentScreen = newScreen;
            currentScreen.Show();
        }

        internal BirdFactory bf { get; set; }
    }
}
