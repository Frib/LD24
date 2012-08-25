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
        public BasicEffect e;
        public SpriteFont font;
        Screen currentScreen;

        public G()
        {
            g = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            currentScreen = new GameScreen(this);
            currentScreen.Show();
        }

        protected override void LoadContent()
        {
            e = new BasicEffect(GraphicsDevice);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("font");
            RM.AddTexture("island", Content.Load<Texture2D>("terrain"));
            RM.AddTexture("grass", Content.Load<Texture2D>("grass"));
            RM.AddTexture("water", Content.Load<Texture2D>("water"));
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            IM.NewState();
            if (IM.IsKeyDown(Keys.Escape))
                this.Exit();

            currentScreen.Update();

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
    }
}
