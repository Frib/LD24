using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LD24
{
    class GameScreen : Screen
    {
        private Camera camera;
        private Island island;
        private int timer;
        private int HiScore;
        private bool paused;
        
        public GameScreen(G g)
        {
            camera = new Camera();
            island = new Island();
            CanTakePhoto = true;
        }

        public override void Update()
        {
            if (!paused)
            {

                if (timer > 0)
                {
                    timer--;
                    if (timer == 0)
                        CanTakePhoto = true;
                }
                if (RM.IsPressed(InputAction.ShowAlbum))
                {
                    g.Showscreen(new PhotoAlbum(this));
                }

                camera.Update();
                island.Update();
            }
            else
            {
                if (RM.IsPressed(InputAction.Accept))
                {
                    g.Showscreen(new MainMenuScreen());
                }
            }

            if (RM.IsPressed(InputAction.Back))
            {
                paused = !paused;

                IM.SnapToCenter = !paused;
                g.IsMouseVisible = paused;
            }

           // camera.position.Y = island.CheckHeightCollision(camera.position);
        }

        public override void Draw()
        {
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true, DepthBufferWriteEnable = true };
            GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;

            camera.Apply(e);

            foreach (var p in e.CurrentTechnique.Passes)
            {
                p.Apply();
            }

            island.Draw();

            spriteBatch.Begin();
            spriteBatch.DrawString(g.font, HiScore.ToString(), new Vector2(16, 16), Color.Yellow);
            spriteBatch.DrawString(g.font, "Species: " + Encyclopedia.UniquesCount, new Vector2(650, 16), Color.Yellow);
            if (RM.IsDown(InputAction.AltFire))
            {
                if (RM.IsPressed(InputAction.Fire) && CanTakePhoto)
                {

                }
                else
                {
                    spriteBatch.Draw(RM.GetTexture("cameraoverlay"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                }
            }
            if (paused)
            {
                spriteBatch.Draw(RM.GetTexture("grass"), new Rectangle((int)(G.Width * 0.25f), (int)(G.Height * 0.4f), (int)(G.Width * 0.5f), (int)(G.Height * 0.2f)), Color.Black);
                spriteBatch.DrawString(g.font, "Game paused. Press " + RM.GetButtons(InputAction.Accept).First().ToString() + " to exit", new Vector2(232, G.Height / 2 - 32), Color.Red);
                spriteBatch.DrawString(g.font, "Press " + RM.GetButtons(InputAction.Back).First().ToString() + " to continue playing", new Vector2(232, G.Height / 2 ), Color.Green);
            }

            spriteBatch.End();
        }

        public override void Show()
        {
            IM.SnapToCenter = true;
            g.IsMouseVisible = false;
            base.Show();
        }

        internal override void AddPhotoData(Photograph pg)
        {
            CanTakePhoto = false;
            RM.PlaySound("snap");
            timer = 60;

            BoundingFrustum bf = new BoundingFrustum(camera.View * camera.ZoomProjection);
            var entities = island.GetEntitiesInView(bf);
            Console.WriteLine(entities.Count());
            var bird = entities.OfType<Bird>().OrderBy(x => (x.Position - island.player.Position).Length()).FirstOrDefault();
            if (bird != null)
            {
                pg.Bird = bird;
                pg.animation = bird.animation;
                pg.Distance = (bird.Position - island.player.Position).Length();
                pg.Splash = entities.OfType<SplashEffect>().Any();
                pg.Heading = bird.GetHeading();
                pg.Zoom = camera.cameraZoom;
                Encyclopedia.AddBird(bird);
            }

            if (pg.CalculateScore() > HiScore)
            {
                HiScore = pg.CalculateScore();
            }
            Console.Write(pg.ToString());

        }
    }
}
